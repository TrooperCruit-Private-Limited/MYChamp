using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MYChamp.DbContexts;
using MYChamp.Models;
using System.Runtime.CompilerServices;

namespace MYChamp.Pages.Employees
{
    public class IndexModel : PageModel
    {
      
        private  readonly UserManager<AppUser> _userManager;
        private readonly MYChampDbContext _db;
        public IndexModel( UserManager<AppUser> userManager,MYChampDbContext db )
        {
           
            _userManager = userManager;
            _db = db;
        }

        public IList<Employee> Employees { get; set; }

        public Employee CurrentEmployee { get; set; }
        public bool IsHR {  get; set; }
        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var data = _db.registerModel.FirstOrDefault(d=>d.EmailId==user.Email);
            IsHR = data != null && data.JobTitle == "HR";

            Employees = new List<Employee>();
            
            if (data != null && data.JobTitle == "HR")
            {
                Employees = await _db.Employees
                    .Where(e=> !e.IsActive)
                    .Include(e => e.Position)
                    .ToListAsync();
                foreach (var employee in Employees)
                {
                    var manager = await _db.Employees
                        .FirstOrDefaultAsync(m => m.EmployeeId == employee.ReportingManagerId);
                    employee.ReportingManagerName = manager?.Name;
                }
            }
            else
            {
                CurrentEmployee = await _db.Employees
            .Include(e => e.Position)
            .FirstOrDefaultAsync(e => e.Email == user.Email && !e.IsActive);

                if (CurrentEmployee != null && CurrentEmployee.ReportingManagerId.HasValue)
                {
                    var manager = await _db.Employees
                        .FirstOrDefaultAsync(m => m.EmployeeId == CurrentEmployee.ReportingManagerId.Value);
                    CurrentEmployee.ReportingManagerName = manager?.Name; 
                }

            }
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
