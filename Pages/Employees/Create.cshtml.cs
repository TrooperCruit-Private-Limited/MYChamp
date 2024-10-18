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
    public class CreateModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly MYChampDbContext _db;

        public CreateModel(UserManager<AppUser> userManager, MYChampDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        [BindProperty]
        public Employee Employee { get; set; }

        public SelectList PositionSelectList { get; set; }
        public SelectList ReportingManagers { get; set; }
        public SelectList ResponsibilitySelection { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var data = await _db.registerModel.FirstOrDefaultAsync(d => d.EmailId == user.Email);

            // Check if the user is authorized (HR role)
            if (data == null || data.JobTitle != "HR")
            {
                return RedirectToPage("/Index");
            }

            // Populate PositionSelectList with positions that are inactive
            PositionSelectList = new SelectList(await _db.Positions
                .Where(p => !p.IsActiveP)
                .ToListAsync(), "Id", "Name");

            // Populate ResponsibilitySelection
            ResponsibilitySelection = new SelectList(await _db.Responsibilities.ToListAsync(), "Id", "ResponsibilityName");

            // Get employees that are inactive and can act as reporting managers
            var employees = await _db.Employees.Where(e => !e.IsActive).ToListAsync();

            // Handle the case when no reporting managers are available
            if (!employees.Any())
            {
                ReportingManagers = new SelectList(new List<SelectListItem>
                {
                    new SelectListItem { Value = "", Text = "-- No Reporting Manager Available --" }
                }, "Value", "Text");
            }
            else
            {
                ReportingManagers = new SelectList(employees, "EmployeeId", "Name");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // Re-populate the dropdowns in case of validation errors
                PositionSelectList = new SelectList(await _db.Positions.Where(p => !p.IsActiveP).ToListAsync(), "Id", "Name");
                ReportingManagers = new SelectList(await _db.Employees.Where(e => !e.IsActive).ToListAsync(), "EmployeeId", "Name");
                ResponsibilitySelection = new SelectList(await _db.Responsibilities.ToListAsync(), "Id", "ResponsibilityName");

                Console.WriteLine($"Responsibility: {ResponsibilitySelection}");

                return Page();
            }

            // Check for duplicate EmployeeId
            var existingEmployee = await _db.Employees
                .FirstOrDefaultAsync(e => e.EmployeeId == Employee.EmployeeId);

            if (existingEmployee != null)
            {
                ModelState.AddModelError("Employee.EmployeeId", "Employee ID must be unique.");

                // Re-populate the dropdowns
                PositionSelectList = new SelectList(await _db.Positions.Where(p => !p.IsActiveP).ToListAsync(), "Id", "Name");
                ReportingManagers = new SelectList(await _db.Employees.Where(e => !e.IsActive).ToListAsync(), "EmployeeId", "Name");
                ResponsibilitySelection = new SelectList(await _db.Responsibilities.ToListAsync(), "Id", "ResponsibilityName");

                return Page();
            }

            // Set remaining leaves based on the selected position
            var selectedPosition = await _db.Positions.FindAsync(Employee.PositionId);
            if (selectedPosition != null)
            {
                Employee.RemainingLeaves = selectedPosition.AllottedLeaves;
            }

            // Add the new employee and selected responsibilities to the database
            await _db.Employees.AddAsync(Employee);
            await _db.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

    }
}
