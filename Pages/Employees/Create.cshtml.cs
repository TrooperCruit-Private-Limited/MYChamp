using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MYChamp.DbContexts;
using MYChamp.Models;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace MYChamp.Pages.Employees
{
    public class CreateModel : PageModel
    {
       
        private readonly UserManager<AppUser> _userManager;
        private readonly MYChampDbContext _db;
        public CreateModel(UserManager<AppUser> userManager,MYChampDbContext db)
        {
           
            _userManager = userManager;
            _db = db;
        }

        [BindProperty]
        public Employee Employee { get; set; }

        public SelectList PositionSelectList { get; set; }   
        public SelectList ReportingManagers {  get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var data = _db.registerModel.FirstOrDefault(d => d.EmailId == user.Email);
            if (data == null || data.JobTitle!="HR") {
                return RedirectToPage("Pages/Index");
            
            }
            PositionSelectList = new SelectList(_db.Positions.Where(a=>!a.IsActiveP), "Id", "Name");
            // ReportingManagers = new SelectList(_db.Employees.Where(a=>!a.IsActive) , "EmployeeId", "Name");

            var employees = _db.Employees.Where(a => !a.IsActive);

          
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
                PositionSelectList = new SelectList(_db.Positions, "Id", "Name");
                return Page();
            }

            var existingEmployee = await _db.Employees
        .FirstOrDefaultAsync(e => e.EmployeeId == Employee.EmployeeId);
            if (existingEmployee != null)
            {
                ModelState.AddModelError("Employee.EmployeeId", "Employee ID must be unique.");

                PositionSelectList = new SelectList(_db.Positions, "Id", "Name");
                ReportingManagers = new SelectList(_db.Employees.Where(a => !a.IsActive), "EmployeeId", "Name");

                return Page();
            }
            var selectedPosition = await _db.Positions.FindAsync(Employee.PositionId);
            if (selectedPosition != null)
            {
                Employee.RemainingLeaves = selectedPosition.AllottedLeaves;
            }
          
             
            await _db.Employees.AddAsync(Employee);
            await _db.SaveChangesAsync();
      
            return RedirectToPage("./Index");
        }
    }
}
