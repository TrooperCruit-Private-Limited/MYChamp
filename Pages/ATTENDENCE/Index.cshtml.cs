using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MYChamp.Controller;
using MYChamp.DbContexts;
using MYChamp.Migrations;
using MYChamp.Models;

namespace MYChamp.Pages.ATTENDENCE
{
    public class IndexModel : PageModel
    {
        private readonly MYChampDbContext _context;

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly UserManager<AppUser> _userManager;
        private object lastPunch;

        public IndexModel(MYChampDbContext context, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IHttpClientFactory httpClientFactory, UserManager<AppUser> userManager
)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _userManager = userManager;


        }

        [BindProperty]
        public AttendenceModel Attendence { get; set; } = default;

        public bool IsAttendanceAdded { get; set; }

        public LeaveApplication LeaveApplications { get; set; }
 
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null) { 
                return NotFound();
            
            }

            var today = DateTime.UtcNow.Date;
            var existingAttendance = await _context.Attendence
                .FirstOrDefaultAsync(a => a.UserID == user.Id && a.DateTime.Date == today);

            Attendence = new AttendenceModel
            {
                UserID = user.Id
            };

            // Check if attendance already exists
            IsAttendanceAdded = existingAttendance != null;

            return Page();
          
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var today = DateTime.UtcNow.Date;
            var existingAttendance = await _context.Attendence
                .FirstOrDefaultAsync(a => a.UserID == user.Id && a.DateTime.Date == today);

            if (existingAttendance != null)
            {
                // Employee has already punched in today
                ModelState.AddModelError(string.Empty, "You have already punched in today.");
                return Page();
            }

            var leaveRequest = await _context.LeaveApplications
                .FirstOrDefaultAsync(l => l.UserID == user.Id && l.DateRequested.Date == today && l.Status == "Approved");

            if (leaveRequest != null)
            {
                // Employee has a permission leave
                Attendence = new AttendenceModel
                {
                    UserID = user.Id,
                    DateTime = today,
                    MarkedAttendence = 0, // Not Present
                    LeaveType = 1 // Permission Leave
                };
                _context.Attendence.Add(Attendence);
            }
            else
            {
                // Validate punch time (between 9 AM and 10 AM)
                //var currentUtcTime = DateTime.UtcNow;
                //var punchStartTime = today.AddHours(9); 
                //var punchEndTime = today.AddHours(10);  

                //if (currentUtcTime >= punchStartTime && currentUtcTime <= punchEndTime)
                //{
                //    // Valid punch time
                //    Attendence = new AttendenceModel
                //    {
                //        UserID = user.Id,
                //        DateTime = currentUtcTime,
                //        MarkedAttendence = 1, // Present
                //        LeaveType = 0 // None
                //    };
                //    _context.Attendence.Add(Attendence);
                //}
                //else
                //{
                    // Invalid punch time, mark as "Not Mentioned"
                    Attendence = new AttendenceModel
                    {
                        UserID = user.Id,
                        DateTime = today,
                        MarkedAttendence = 0, // Not Present
                        LeaveType = 2 // Not Mentioned
                    };
                    _context.Attendence.Add(Attendence);
                // }
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("Success");



            //Attendence.DateTime = DateTime.UtcNow;
            //_context.Attendence.Add(Attendence);
            //await _context.SaveChangesAsync();


            //return RedirectToPage("Success");
        }
    }

}
