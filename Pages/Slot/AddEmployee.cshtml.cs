using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MYChamp.DbContexts;
using MYChamp.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MYChamp.Pages.Slot
{
    public class AddEmployeeModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly MYChampDbContext _db;

        public string Name { get; set; }
        public string JobTitle { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        [BindProperty]
        public RegisterModel _register { get; set; }

        [BindProperty]
        public string SlotDate { get; set; }

        [BindProperty]
        public string SlotStart { get; set; }

        [BindProperty]
        public string SlotEnd { get; set; }

        public string ErrorMessage { get; set; }

        private readonly TimeSpan OfficeStartTime = TimeSpan.Parse("10:30"); // 10:30 AM
        private readonly TimeSpan OfficeEndTime = TimeSpan.Parse("18:00"); // 6:00 PM

        public AddEmployeeModel(UserManager<AppUser> userManager, MYChampDbContext dbContext)
        {
            _userManager = userManager;
            _db = dbContext;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var employeeDetails = await _db.registerModel
                .Where(e => e.EmailId == currentUser.Email)
                .FirstOrDefaultAsync();

            if (employeeDetails != null)
            {
                _register = new RegisterModel
                {
                    FirstName = employeeDetails.FirstName,
                    LastName = employeeDetails.LastName,
                    PhoneNumber = employeeDetails.PhoneNumber,
                    JobTitle = employeeDetails.JobTitle,
                    EmailId = employeeDetails.EmailId
                };
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Console.WriteLine("Form submitted");

            if (!ModelState.IsValid)
            {
                foreach (var key in ModelState.Keys)
                {
                    var state = ModelState[key];
                    foreach (var error in state.Errors)
                    {
                        Console.WriteLine($"Validation Error in {key}: {error.ErrorMessage}");
                    }
                }
            }

            var currentUser = await _userManager.GetUserAsync(User);
            var employeeDetails = await _db.registerModel
                .Where(e => e.EmailId == currentUser.Email)
                .FirstOrDefaultAsync();

            if (employeeDetails == null)
            {
                ModelState.AddModelError(string.Empty, "User details not found.");
                return Page();
            }

            if (!DateTime.TryParse(SlotDate, out DateTime slotDate) ||
                !TimeSpan.TryParse(SlotStart, out TimeSpan slotStart) ||
                !TimeSpan.TryParse(SlotEnd, out TimeSpan slotEnd))
            {
                ModelState.AddModelError(string.Empty, "Invalid date or time format.");
                return Page();
            }

            // 🔹 Ensure SlotDate is treated as UTC
            slotDate = DateTime.SpecifyKind(slotDate, DateTimeKind.Utc);

            DateTime today = DateTime.UtcNow.Date;
            DateTime maxAllowedDate = today.AddDays(2);

            if (slotDate < today || slotDate > maxAllowedDate)
            {
                ModelState.AddModelError(string.Empty, "You can only book slots for today, tomorrow, or the day after.");
                return Page();
            }

            if (slotStart < OfficeStartTime || slotEnd > OfficeEndTime || slotStart >= slotEnd)
            {
                ModelState.AddModelError(string.Empty, "Invalid slot time.");
                return Page();
            }

            var existingEmployee = await _db.EmployeeLists
                .Where(e => e.Email == currentUser.Email)
                .FirstOrDefaultAsync();

            if (existingEmployee != null)
            {
                // **Update existing employee**
                existingEmployee.SlotDate = slotDate;
                existingEmployee.SlotStart = slotStart;
                existingEmployee.SlotEnd = slotEnd;
                existingEmployee.UpdatedAt = DateTime.UtcNow; // ✅ Ensure UTC

                _db.EmployeeLists.Update(existingEmployee);
            }
            else
            {
                // **Insert new employee record**
                var newEmployee = new EmployeeList
                {
                    Name = $"{employeeDetails.FirstName} {employeeDetails.LastName}",
                    JobTitle = employeeDetails.JobTitle,
                    Email = employeeDetails.EmailId,
                    Phone = employeeDetails.PhoneNumber,
                    SlotDate = slotDate,
                    SlotStart = slotStart,
                    SlotEnd = slotEnd,
                    CreatedAt = DateTime.UtcNow, // ✅ Ensure UTC
                    UpdatedAt = DateTime.UtcNow
                };

                await _db.EmployeeLists.AddAsync(newEmployee);
            }

            int changes = await _db.SaveChangesAsync();
            Console.WriteLine($"Database Changes: {changes}");

            return RedirectToPage("/Slot/Index");
        }
    }
}
