using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MYChamp.DbContexts;
using MYChamp.Models;
using System.Collections.Generic;
using System.Linq;

namespace MYChamp.Pages.Leaves
{
    public class IndexModel : PageModel
    {
        
        private readonly UserManager<AppUser> _userManager;
        private readonly MYChampDbContext _db;
        public IndexModel( UserManager<AppUser> userManager, MYChampDbContext db)
        {
           
            _userManager = userManager;
            _db = db;
        }

        public IList<LeaveApplication> LeaveApplications { get; set; }
        public bool IsHR { get; set; }
        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var data = _db.registerModel.FirstOrDefault(d => d.EmailId == user.Email);
            IsHR = data != null && data.JobTitle == "HR";
            if (data != null && data.JobTitle == "HR")
            {
                LeaveApplications = _db.LeaveApplications
                .Include(l => l.Employee)
                .Include(l => l.Manager)
                .ToList();
            }
            else
            {
                LeaveApplications = _db.LeaveApplications.Where(a => a.Employee.Email == user.Email)
                     .Include(l => l.Employee)
                     .Include(l => l.Manager)
                     .ToList();
            }
        }
    }
}
