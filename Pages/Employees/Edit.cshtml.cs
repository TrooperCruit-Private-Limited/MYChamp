using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MYChamp.DbContexts;
using MYChamp.Models;

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
        public SelectList PositionSelectList { get; set; }
        public SelectList ReportingManagers { get; set; }
        public SelectList ResponsibilitySelection { get; set; }


        public bool IsHR { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var user = await _userManager.GetUserAsync(User);
            var data = _db.registerModel.FirstOrDefault(d => d.EmailId == user.Email);
            IsHR = data != null && data.JobTitle == "HR";
            if (id == null)
            {
                return NotFound();
            }

            Employee = await _db.Employees
                      .Include(e => e.Position)
                      .FirstOrDefaultAsync(m => m.Id == id);

            if (Employee == null)
            {
                return NotFound();
            }

            PositionSelectList = new SelectList(_db.Positions, "Id", "Name");
            ReportingManagers = new SelectList(_db.Employees, "EmployeeId", "Name");
            ResponsibilitySelection = new SelectList(_db.Responsibilities, "Id", "ResponsibilityName");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _db.Update(Employee);
            await _db.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}
