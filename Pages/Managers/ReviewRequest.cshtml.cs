using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MYChamp.DbContexts;
using MYChamp.Models;

namespace MYChamp.Pages.Managers
{
    public class ReviewRequestModel : PageModel
    {

       
      
        private readonly UserManager<AppUser> _userManager;
        private readonly MYChampDbContext _db;
       
        public List<LeaveApplication> PendingLeaveRequests { get; set; }

        public ReviewRequestModel( UserManager<AppUser> userManager, MYChampDbContext db)
        {
            
            _userManager = userManager;
           _db = db;
        }

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var employee = _db.Employees
                .Include(e => e.Position)
                .FirstOrDefault(e => e.Email == user.Email);
            
          // var manager = await _context.Employees.FirstOrDefaultAsync(a=>a.EmployeeId==a.ReportingManagerId);
           
            var data=_db.registerModel.FirstOrDefault(e => e.EmailId == user.Email);
            if (data != null)
            {

                if (data.JobTitle == "HR")
                {

                    PendingLeaveRequests = _db.LeaveApplications
                        .Include(l => l.Employee)
                        .Where(l => l.Status == "Requested")
                        .ToList();
                }
                else 
                {

                    var managerId = employee.Id;
                    PendingLeaveRequests = _db.LeaveApplications
                        .Include(l => l.Employee)
                        .Where(l => l.ManagerId == managerId && l.Status == "Requested")
                        .ToList();
                }
                foreach (var request in PendingLeaveRequests)
                {
                    if (request.Employee.ReportingManagerId.HasValue)
                    {
                        var manager = await _db.Employees
                            .FirstOrDefaultAsync(m => m.EmployeeId == request.Employee.ReportingManagerId.Value);
                        request.Employee.ReportingManagerName = manager?.Name; // Assuming you have a property for this
                    }
                }
            }  
        }



        public async Task<IActionResult> OnPostApproveAsync(int id)
        {
            var leaveApplication = _db.LeaveApplications
                                    .Include(l => l.Employee)
                                    .FirstOrDefault(l => l.Id == id);
            if (leaveApplication != null)
            {
                leaveApplication.Status = "Approved";

               
                var employee = leaveApplication.Employee;

                if (employee != null)
                {
                    employee.RemainingLeaves -= leaveApplication.NumberOfDays;
                    _db.Employees.Update(employee);
                }

                _db.Update(leaveApplication);
                await _db.SaveChangesAsync();
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostRejectAsync(int id)
        {
            var leaveApplication = _db.LeaveApplications.FirstOrDefault(l => l.Id == id);
            if (leaveApplication != null)
            {
                leaveApplication.Status = "Rejected";
                _db.Update(leaveApplication);
                await _db.SaveChangesAsync();
            }

            return RedirectToPage();
        }

    }
}
