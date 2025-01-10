using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MYChamp.DbContexts;
using MYChamp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MYChamp.Pages.Employees
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly MYChampDbContext _db;

        public IndexModel(UserManager<AppUser> userManager, MYChampDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        public IList<Employee> Employees { get; set; }
        public Employee CurrentEmployee { get; set; }
        public IList<Article> Articles { get; set; }
        public bool IsHR { get; set; }
        public Dictionary<int, string> ResponsibilityDictionary { get; set; }

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user?.Email == null)
            {
                return;
            }

            // Check if the user has an HR role
            var data = await _db.registerModel.FirstOrDefaultAsync(d => d.EmailId == user.Email);
            IsHR = data?.JobTitle == "HR";

            // Pre-load responsibilities dictionary
            ResponsibilityDictionary = await _db.Responsibilities
                .ToDictionaryAsync(r => r.Id, r => r.ResponsibilityName);

            if (IsHR)
            {
                Employees = await _db.Employees
                    .Where(e => !e.IsActive)
                    .Include(e => e.Position)
                    .ToListAsync();

                // Set ReportingManagerName for each employee
                foreach (var employee in Employees)
                {
                    var manager = await _db.Employees
                        .FirstOrDefaultAsync(m => m.EmployeeId == employee.ReportingManagerId);
                    employee.ReportingManagerName = manager?.Name;

                    // Initialize SelectedResponsibilityIds if null
                    employee.SelectedResponsibilityIds ??= new List<int>();
                }
            }
            else
            {
                CurrentEmployee = await _db.Employees
                    .Include(e => e.Position)
                    .FirstOrDefaultAsync(e => e.Email == user.Email && !e.IsActive);

                if (CurrentEmployee?.ReportingManagerId.HasValue == true)
                {
                    var manager = await _db.Employees
                        .Where(m => m.EmployeeId == CurrentEmployee.ReportingManagerId.Value)
                        .Select(m => m.Name)
                        .FirstOrDefaultAsync();
                    CurrentEmployee.ReportingManagerName = manager;
                }


                // CurrentEmployee?.SelectedResponsibilityIds ??= new List<int>();
            }

            // Load articles related to the user
            Articles = await _db.Article
                .Where(a => a.UserEmail == user.Email && !a.IsArchived)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostArchiveAsync(int id)
        {
            var employee = await _db.Employees.FindAsync(id);
            if (employee != null)
            {
                employee.IsActive = true;
                _db.Update(employee);
                await _db.SaveChangesAsync();
            }
            return RedirectToPage();
        }
    }
}
