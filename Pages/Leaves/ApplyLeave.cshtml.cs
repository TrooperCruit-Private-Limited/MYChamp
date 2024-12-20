using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MYChamp.DbContexts;
using MYChamp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Linq;

namespace MYChamp.Pages
{
    public class ApplyLeaveModel : PageModel
    {
        private readonly MYChampDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public ApplyLeaveModel(MYChampDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public LeaveApplication LeaveApplication { get; set; }

        public SelectList ManagersList { get; set; }
        public Employee Employee { get; set; }  

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToPage("/Error"); 
            }

          
            Employee = await _context.Employees
                                     .Include(e => e.Position)
                                     .FirstOrDefaultAsync(e => e.Email == user.Email);

            
            if (Employee == null)
            {
                return RedirectToPage("/Error");
            }

            // Initialize LeaveApplication if null
            LeaveApplication = new LeaveApplication()
            {
                EmployeeId = Employee.EmployeeId,
                Employee = Employee,
                ManagerId =Employee.ReportingManagerId.HasValue
                        ? (int)Employee.ReportingManagerId.Value : 0

            };

            // Populate the Manager dropdown based on the ReportingManagerId
            //if (Employee.ReportingManagerId.HasValue)
            //{
            //    var manager = await _context.Employees.FirstOrDefaultAsync(e => e.Id == Employee.ReportingManagerId.Value);
            //    if (manager != null)
            //    {
            //        ManagersList = new SelectList(new[] { manager }, "Id", "Name");
            //        LeaveApplication.ManagerId = manager.Id; // Set the default manager
            //    }
            //}

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

           
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.EmployeeId == LeaveApplication.EmployeeId);
            if (employee == null)
            {
                ModelState.AddModelError(string.Empty, "Employee not found.");
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);


            LeaveApplication.Employee = employee;

            LeaveApplication.UserID = user.Id;

           
            LeaveApplication.FromDate = DateTime.SpecifyKind(LeaveApplication.FromDate, DateTimeKind.Utc);
            LeaveApplication.ToDate = DateTime.SpecifyKind(LeaveApplication.ToDate, DateTimeKind.Utc);
            LeaveApplication.DateRequested = DateTime.UtcNow;

            if (LeaveApplication.IsHalfDay)
            {
                if (string.IsNullOrEmpty(LeaveApplication.Session))
                {
                    ModelState.AddModelError(string.Empty, "Please select a session for half-day leave.");
                    return Page();
                }

                // Adjust the leave duration based on the session (morning or afternoon)
                // Assuming a half-day leave counts as 0.5 day
                LeaveApplication.NumberOfDays = 0.5;
            }
            else
            {
                LeaveApplication.NumberOfDays = CalculateLeaveDaysExcludingWeekends(LeaveApplication.FromDate, LeaveApplication.ToDate);
            }

            if (employee.RemainingLeaves < LeaveApplication.NumberOfDays)
            {
                ModelState.AddModelError(string.Empty, "Not enough remaining leaves.");
                return Page();
            }

            LeaveApplication.Status = "Requested";

            LeaveApplication.ManagerId = employee.ReportingManagerId.HasValue
                        ? (int)employee.ReportingManagerId.Value : 0;

            var manager = await _context.Employees.FirstOrDefaultAsync(m => m.EmployeeId == LeaveApplication.ManagerId);

            if (manager == null)
            {
                ModelState.AddModelError(string.Empty, "Manager not found.");
                return Page();
            }

            LeaveApplication.Manager = manager;

            
            _context.LeaveApplications.Add(LeaveApplication);
            _context.Employees.Update(employee);

            await _context.SaveChangesAsync();
            return RedirectToPage("Index");
        }

        private int CalculateLeaveDaysExcludingWeekends(DateTime fromDate, DateTime toDate)
        {
            int totalDays = 0;
            DateTime currentDay = fromDate;

            while (currentDay <= toDate)
            {
                if (currentDay.DayOfWeek != DayOfWeek.Saturday && currentDay.DayOfWeek != DayOfWeek.Sunday)
                {
                    totalDays++;
                }
                currentDay = currentDay.AddDays(1);
            }

            return totalDays;
        }
    }
}
