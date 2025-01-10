using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MYChamp.Controller;
using MYChamp.DbContexts;
using MYChamp.Models;
using Microsoft.Extensions.Logging;

namespace MYChamp.Pages.ATTENDENCE
{
    public class IndexModel : PageModel
    {
        private readonly MYChampDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(
            MYChampDbContext context,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory,
            UserManager<AppUser> userManager,
            ILogger<IndexModel> logger)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _userManager = userManager;
            _logger = logger;
        }

        [BindProperty]
        public AttendenceModel Attendence { get; set; } = default;

        public bool IsAttendanceAdded { get; set; }

        public LeaveApplication LeaveApplications { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                if (user == null)
                {
                    _logger.LogWarning("User not found while accessing attendance.");
                    return NotFound();
                }

                var today = DateTime.UtcNow.Date;
                var existingAttendance = await _context.Attendence
                    .FirstOrDefaultAsync(a => a.UserID == user.Id && a.DateTime.Date == today);

                Attendence = new AttendenceModel
                {
                    UserID = user.Id
                };

                IsAttendanceAdded = existingAttendance != null;

                // Check for last attendance date
                var lastAttendance = await _context.Attendence
                    .Where(a => a.UserID == user.Id)
                    .OrderByDescending(a => a.DateTime)
                    .FirstOrDefaultAsync();

                if (lastAttendance != null)
                {
                    var yesterday = today.AddDays(-1);
                    if (lastAttendance.DateTime.Date < yesterday)
                    {
                        // Check if leave application exists
                        var leaveRequest = await _context.LeaveApplications
                            .FirstOrDefaultAsync(l => l.UserID == user.Id && l.DateRequested.Date == yesterday && l.Status == "Approved");

                        if (leaveRequest == null)
                        {
                            // Mark attendance as not mentioned
                            var missedAttendance = new AttendenceModel
                            {
                                UserID = user.Id,
                                DateTime = yesterday,
                                MarkedAttendence = 0, // Not Present
                                LeaveType = 2 // Not Mentioned
                            };
                            _context.Attendence.Add(missedAttendance);
                            await _context.SaveChangesAsync();
                            _logger.LogInformation("Missed attendance marked as 'Not Mentioned' for {UserId} on {Date}.", user.Id, yesterday);
                        }
                    }
                }

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching attendance details.");
                return StatusCode(500, "Internal server error.");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                if (user == null)
                {
                    _logger.LogWarning("User not found while submitting attendance.");
                    return NotFound();
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for attendance submission.");
                    return Page();
                }

                var today = DateTime.UtcNow.Date;
                var existingAttendance = await _context.Attendence
                    .FirstOrDefaultAsync(a => a.UserID == user.Id && a.DateTime.Date == today);

                if (existingAttendance != null)
                {
                    ModelState.AddModelError(string.Empty, "You have already punched in today.");
                    _logger.LogWarning("User {UserId} tried to punch in twice on {Date}.", user.Id, today);
                    return Page();
                }

                var leaveRequest = await _context.LeaveApplications
                    .FirstOrDefaultAsync(l => l.UserID == user.Id && l.DateRequested.Date == today && l.Status == "Approved");

                if (leaveRequest != null)
                {
                    Attendence = new AttendenceModel
                    {
                        UserID = user.Id,
                        DateTime = today,
                        MarkedAttendence = 0, // Not Present
                        LeaveType = 1 // Permission Leave
                    };
                    _logger.LogInformation("Permission leave marked for {UserId} on {Date}.", user.Id, today);
                }
                else
                {
                    Attendence = new AttendenceModel
                    {
                        UserID = user.Id,
                        DateTime = today,
                        MarkedAttendence = 0, // Not Present
                        LeaveType = 2 // Not Mentioned
                    };
                    _logger.LogInformation("Attendance marked as 'Not Mentioned' for {UserId} on {Date}.", user.Id, today);
                }

                _context.Attendence.Add(Attendence);
                await _context.SaveChangesAsync();
                return RedirectToPage("Success");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while submitting attendance.");
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}
