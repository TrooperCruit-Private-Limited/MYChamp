using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MYChamp.DbContexts;
using MYChamp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MYChamp.Pages.Slot
{
    public class MySlotsModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly MYChampDbContext _db;

        public MySlotsModel(UserManager<AppUser> userManager, MYChampDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        public List<EmployeeList> UserSlots { get; set; } = new();
        public List<SlotBookingDetails> BookedByOthers { get; set; } = new();
        public List<SlotBookingDetails> BookedByMe { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
                return RedirectToPage("/Account/Login");

            // Fetch User's Own Slots (Created by User)
            UserSlots = await _db.EmployeeLists
                .Where(e => e.Email == currentUser.Email)
                .ToListAsync();

            // Fetch Slots Booked by Others (User's slots that someone else booked)
            BookedByOthers = await _db.SlotBookingDetails
                .Where(s => s.Employee.Email == currentUser.Email && s.BookedByEmail != null)
                .Include(s => s.Employee)
                .ToListAsync();

            // Fetch Slots That User Has Booked (User booked someone’s slot)
            BookedByMe = await _db.SlotBookingDetails
                .Where(s => s.BookedByEmail == currentUser.Email)
                .Include(s => s.Employee)
                .ToListAsync();

            return Page();
        }

        // Delete Own Slot (Only if not booked)
        public async Task<IActionResult> OnPostDeleteSlotAsync(int id)
        {
            var slot = await _db.EmployeeLists.FindAsync(id);
            if (slot == null)
                return new JsonResult(new { success = false, message = "Slot not found." });

            bool isBooked = await _db.SlotBookingDetails.AnyAsync(s => s.EmployeeId == id && s.IsBooked);
            if (isBooked)
                return new JsonResult(new { success = false, message = "Slot is booked and cannot be deleted." });

            _db.EmployeeLists.Remove(slot);
            await _db.SaveChangesAsync();

            return new JsonResult(new { success = true });
        }

        // Cancel Booking (Only if user booked a slot)
        public async Task<IActionResult> OnPostCancelBookingAsync(int id)
        {
            var booking = await _db.SlotBookingDetails.FindAsync(id);
            if (booking == null)
                return new JsonResult(new { success = false, message = "Booking not found." });

            // Mark booking as cancelled
            booking.IsBooked = false;
            booking.IsCanceled = true;

            _db.SlotBookingDetails.Update(booking);
            await _db.SaveChangesAsync();

            return new JsonResult(new { success = true });
        }
    }
}
