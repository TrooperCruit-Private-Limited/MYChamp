using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using MYChamp.DbContexts;
using MYChamp.Models;
using Microsoft.EntityFrameworkCore;

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

        // Store comma-separated responsibility IDs
        [BindProperty]
        public string ResponsibilityIds { get; set; }

        public SelectList PositionSelectList { get; set; }
        public SelectList ReportingManagers { get; set; }
        public List<Responsibility> ResponsibilityList { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var data = await _db.registerModel.FirstOrDefaultAsync(d => d.EmailId == user.Email);
            if (data == null || data.JobTitle != "HR")
            {
                return RedirectToPage("./Index");
            }

            PositionSelectList = new SelectList(_db.Positions.Where(a => !a.IsActiveP), "Id", "Name");

            var employees = await _db.Employees.Where(a => !a.IsActive).ToListAsync();
            ReportingManagers = employees.Any()
                ? new SelectList(employees, "EmployeeId", "Name")
                : new SelectList(new List<SelectListItem>
                {
                    new SelectListItem { Value = "", Text = "-- No Reporting Manager Available --" }
                }, "Value", "Text");

            ResponsibilityList = await _db.Responsibilities.ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                PositionSelectList = new SelectList(_db.Positions, "Id", "Name");
                ReportingManagers = new SelectList(_db.Employees.Where(a => !a.IsActive), "EmployeeId", "Name");
                ResponsibilityList = await _db.Responsibilities.ToListAsync();
                return Page();
            }

            var existingEmployee = await _db.Employees
                .FirstOrDefaultAsync(e => e.EmployeeId == Employee.EmployeeId);
            if (existingEmployee != null)
            {
                ModelState.AddModelError("Employee.EmployeeId", "Employee ID must be unique.");
                PositionSelectList = new SelectList(_db.Positions, "Id", "Name");
                ReportingManagers = new SelectList(_db.Employees.Where(a => !a.IsActive), "EmployeeId", "Name");
                ResponsibilityList = await _db.Responsibilities.ToListAsync();
                return Page();
            }

            var selectedPosition = await _db.Positions.FindAsync(Employee.PositionId);
            if (selectedPosition != null)
            {
                Employee.RemainingLeaves = selectedPosition.AllottedLeaves;
            }

            // Convert ResponsibilityIds to a list of integers
            var responsibilityIds = ResponsibilityIds.Split(',')
                .Select(int.Parse).ToList();
            Employee.SelectedResponsibilityIds = responsibilityIds;

            await _db.Employees.AddAsync(Employee);
            await _db.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
