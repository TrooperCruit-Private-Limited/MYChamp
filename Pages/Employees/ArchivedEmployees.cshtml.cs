using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MYChamp.DbContexts;
using MYChamp.Models;

namespace MYChamp.Pages.Employees
{
    public class ArchivedEmployeesModel : PageModel
    {
       
        private readonly UserManager<AppUser> _userManager;
        private readonly MYChampDbContext _db;

        public ArchivedEmployeesModel( UserManager<AppUser> userManager, MYChampDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        public IList<Employee> Employees { get; set; }

        public bool IsHR { get; set; }

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var data = _db.registerModel.FirstOrDefault(d => d.EmailId == user.Email);
            IsHR = data != null && data.JobTitle == "HR";

            if (IsHR)
            {
                Employees = await _db.Employees
                    .Where(e => e.IsActive)  // Fetch only archived employees
                    .Include(e => e.Position)
                    .ToListAsync();
            }
            else
            {
                Employees = new List<Employee>(); // No non-HR should view archived employees
            }
        }

        // Optional handler to unarchive an employee if needed
        public async Task<IActionResult> OnPostUnarchiveAsync(int id)
        {
            var employee = await _db.Employees.FindAsync(id);
            if (employee != null)
            {
                employee.IsActive = false; // Set IsArchived to false to restore the employee
                _db.Update(employee);
                await _db.SaveChangesAsync();
            }
            return RedirectToPage();
        }
    }
}
