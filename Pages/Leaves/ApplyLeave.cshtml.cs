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
                        ? (int)Employee.ReportingManagerId.Value : 0,

                UserID = user.Id

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
            LeaveApplication.DateRequested = DateTime.UtcNow;

            LeaveApplication.FromDate = DateTime.SpecifyKind(LeaveApplication.FromDate, DateTimeKind.Utc);
            LeaveApplication.ToDate = DateTime.SpecifyKind(LeaveApplication.ToDate, DateTimeKind.Utc);

            // Create a list to hold the leave details for each day
            List<LeaveDetail> leaveDetails = new List<LeaveDetail>();

            DateTime currentDay = LeaveApplication.FromDate;
            double totalLeaveDays = 0;

            while (currentDay <= LeaveApplication.ToDate)
            {
                string leaveType = Request.Form[$"leaveType_{currentDay:yyyy-MM-dd}"];

                // Add a leave detail entry for each day
                if (!string.IsNullOrEmpty(leaveType))
                {
                    LeaveDetail leaveDetail = new LeaveDetail
                    {
                        LeaveDate = currentDay,
                        LeaveType = leaveType,
                        LeaveApplicationId = user.Id
                    };

                    leaveDetails.Add(leaveDetail);

                    // Calculate total leave days based on the leave type
                    //if (leaveType == "1")  // Full Day
                    //{
                    //    totalLeaveDays += 1.0;
                    //}
                    //else if (leaveType == "0.5M" || leaveType == "0.5A")  // Half Day
                    //{
                    //    totalLeaveDays += 0.5;
                    //}
                }

                currentDay = currentDay.AddDays(1);
            }

            // Exclude weekends from the total leave days
            totalLeaveDays = CalculateLeaveDaysExcludingWeekends(LeaveApplication.FromDate, LeaveApplication.ToDate, leaveDetails);

            //if (employee.RemainingLeaves < totalLeaveDays)
            //{
            //    ModelState.AddModelError(string.Empty, "Not enough remaining leaves.");
            //    return Page();
            //}

            LeaveApplication.NumberOfDays = totalLeaveDays;
            LeaveApplication.Status = "Requested";
            LeaveApplication.ManagerId = employee.ReportingManagerId.HasValue ? employee.ReportingManagerId.Value : 0;

            var manager = await _context.Employees.FirstOrDefaultAsync(m => m.EmployeeId == LeaveApplication.ManagerId);
            if (manager == null)
            {
                ModelState.AddModelError(string.Empty, "Manager not found.");
                return Page();
            }

            LeaveApplication.Manager = manager;

            // Save the leave application and leave details
            _context.LeaveApplications.Add(LeaveApplication);

            // Add the leave details for each day to the database
            foreach (var detail in leaveDetails)
            {
                _context.LeaveDetails.Add(detail);
            }

            // Update the employee's remaining leaves
            employee.RemainingLeaves -= (int)LeaveApplication.NumberOfDays;
            _context.Employees.Update(employee);

            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }

        private double CalculateLeaveDaysExcludingWeekends(DateTime fromDate, DateTime toDate, List<LeaveDetail> leaveDetails)
        {
            double totalDays = 0;

            // Loop through the leave details to exclude weekends
            foreach (var detail in leaveDetails)
            {
                if (detail.LeaveDate.DayOfWeek != DayOfWeek.Saturday && detail.LeaveDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    totalDays += (detail.LeaveType == "1") ? 1.0 : 0.5;  // Full day or half day
                }
            }

            return totalDays;
        }
    }
}
