using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MYChamp.DbContexts;
using MYChamp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MYChamp.Pages.Employees
{
    public class EditModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly MYChampDbContext _db;

        public EditModel(UserManager<AppUser> userManager, MYChampDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        [BindProperty]
        public Employee Employee { get; set; }

        [BindProperty]
        public string ResponsibilityIds { get; set; }

        public SelectList PositionSelectList { get; set; }
        public SelectList ReportingManagers { get; set; }
        public List<Responsibility> ResponsibilityList { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            // Get the logged-in user and check if they are HR
            var user = await _userManager.GetUserAsync(User);
            var data = await _db.registerModel.FirstOrDefaultAsync(d => d.EmailId == user.Email);
            if (data == null || data.JobTitle != "HR")
                return RedirectToPage("./Index");

            // Load the employee record
            Employee = await _db.Employees
                .Include(e => e.Position)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (Employee == null)
                return NotFound();

            // Convert the responsibilities to a comma-separated string
            ResponsibilityIds = string.Join(",", Employee.SelectedResponsibilityIds);

            // Load select lists
            PositionSelectList = new SelectList(_db.Positions, "Id", "Name");
            ReportingManagers = new SelectList(_db.Employees, "EmployeeId", "Name");
            ResponsibilityList = await _db.Responsibilities.ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                PositionSelectList = new SelectList(_db.Positions, "Id", "Name");
                ReportingManagers = new SelectList(_db.Employees, "EmployeeId", "Name");
                ResponsibilityList = await _db.Responsibilities.ToListAsync();
                return Page();
            }

            // Fetch the existing employee record
            var employeeToUpdate = await _db.Employees.FirstOrDefaultAsync(e => e.EmployeeId == Employee.EmployeeId);

            if (employeeToUpdate == null)
                return NotFound();

            // Update employee details
            employeeToUpdate.EmployeeId = Employee.EmployeeId;
            employeeToUpdate.Name = Employee.Name;
            employeeToUpdate.Email = Employee.Email;
            employeeToUpdate.Salary = Employee.Salary;
            employeeToUpdate.PositionId = Employee.PositionId;
            employeeToUpdate.ReportingManagerId = Employee.ReportingManagerId;

            // Convert ResponsibilityIds to a list of integers and update
            var responsibilityIds = ResponsibilityIds.Split(',')
                .Select(int.Parse).ToList();
            employeeToUpdate.SelectedResponsibilityIds = responsibilityIds;

            // Save changes
            _db.Update(employeeToUpdate);
            await _db.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
