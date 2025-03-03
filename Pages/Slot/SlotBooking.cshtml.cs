using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MYChamp.DbContexts;
using MYChamp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MYChamp.Pages.Slot
{
    public class SlotBookingModel : PageModel
    {
        [BindProperty]
        public EmployeeList Employee { get; set; }

        [BindProperty]
        public DateTime SlotDate { get; set; }

        public List<SlotBookingDetails> AvailableSlots { get; set; }
        public bool IsBookingSuccessful { get; set; } = false;

        private readonly MYChampDbContext _context;

        public SlotBookingModel(MYChampDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int employeeId, DateTime date)
        {
            Employee = await _context.EmployeeLists.FindAsync(employeeId);
            if (Employee == null) return NotFound();

            SlotDate = date;
            AvailableSlots = new List<SlotBookingDetails>();

            TimeSpan startTime = Employee.SlotStart;
            TimeSpan endTime = Employee.SlotEnd;

            while (startTime < endTime)
            {
                if (startTime.Add(TimeSpan.FromMinutes(15)) <= endTime)
                {
                    AvailableSlots.Add(new SlotBookingDetails
                    {
                        EmployeeId = employeeId,
                        SlotDate = date,
                        StartTime = startTime,
                        EndTime = startTime.Add(TimeSpan.FromMinutes(15)),
                        IsBooked = _context.SlotBookingDetails.Any(s =>
                                    s.EmployeeId == employeeId &&
                                    s.SlotDate == date &&
                                    s.StartTime == startTime)
                    });

                    startTime = startTime.Add(TimeSpan.FromMinutes(15));
                }

                if (startTime.Add(TimeSpan.FromMinutes(15)) <= endTime)
                {
                    AvailableSlots.Add(new SlotBookingDetails
                    {
                        EmployeeId = employeeId,
                        SlotDate = date,
                        StartTime = startTime,
                        EndTime = startTime.Add(TimeSpan.FromMinutes(15)),
                        IsBooked = _context.SlotBookingDetails.Any(s =>
                                    s.EmployeeId == employeeId &&
                                    s.SlotDate == date &&
                                    s.StartTime == startTime)
                    });

                    startTime = startTime.Add(TimeSpan.FromMinutes(15));
                }

                if (startTime.Add(TimeSpan.FromMinutes(30)) <= endTime)
                {
                    AvailableSlots.Add(new SlotBookingDetails
                    {
                        EmployeeId = employeeId,
                        SlotDate = date,
                        StartTime = startTime,
                        EndTime = startTime.Add(TimeSpan.FromMinutes(30)),
                        IsBooked = _context.SlotBookingDetails.Any(s =>
                                    s.EmployeeId == employeeId &&
                                    s.SlotDate == date &&
                                    s.StartTime == startTime)
                    });

                    startTime = startTime.Add(TimeSpan.FromMinutes(30));
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int EmployeeId, DateTime SlotDate, TimeSpan StartTime, TimeSpan EndTime)
        {
            var userEmail = User.Identity.Name;

            var currentUser = await _context.registerModel
                .Where(u => u.EmailId == userEmail)
                .Select(u => new { u.FirstName, u.EmailId, u.PhoneNumber })
                .FirstOrDefaultAsync();

            if (currentUser == null)
                return BadRequest("Error: User not found in RegisterModel.");

            bool isAlreadyBooked = await _context.SlotBookingDetails
                .AnyAsync(s => s.EmployeeId == EmployeeId &&
                               s.SlotDate == SlotDate &&
                               s.StartTime == StartTime);

            if (isAlreadyBooked)
                return BadRequest("Error: Slot is already booked.");

            var booking = new SlotBookingDetails
            {
                EmployeeId = EmployeeId,
                SlotDate = SlotDate.ToUniversalTime(),  // Convert to UTC
                StartTime = StartTime,
                EndTime = EndTime,
                IsBooked = true,
                BookedBy = currentUser.FirstName,
                BookedByEmail = currentUser.EmailId,
                BookedByPhone = currentUser.PhoneNumber,
                CreatedAt = DateTime.UtcNow,  // ✅ Convert to UTC
                UpdatedAt = DateTime.UtcNow   // ✅ Convert to UTC
            };

            _context.SlotBookingDetails.Add(booking);
            await _context.SaveChangesAsync();

            IsBookingSuccessful = true;

            return RedirectToPage(new { employeeId = EmployeeId, date = SlotDate });
        }

    }
}
