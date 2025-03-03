using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MYChamp.DbContexts;
using MYChamp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MYChamp.Pages.Slot
{
    public class IndexModel : PageModel
    {
        private readonly MYChampDbContext _dbContext;

        public IndexModel(MYChampDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<EmployeeList> Employees { get; set; } = new List<EmployeeList>(); // ✅ No NullReferenceException
        public string loggedInUserEmail { get; set; } = string.Empty;


        public IActionResult OnGet()
        {
            DateTime today = DateTime.UtcNow.Date;  // 🔹 Convert to UTC

            // Office timings
            TimeSpan startOfficeTime = new(10, 30, 0); // 10:30 AM
            TimeSpan endOfficeTime = new(19, 30, 0);   // 7:30 PM

            // Get logged-in user's email safely
            loggedInUserEmail = User?.Identity?.Name ?? string.Empty;

            // Fetch only active slots (today or future)
            Employees = _dbContext.EmployeeLists
                .Where(e => e.SlotDate >= today)  // 🔹 Ensure date comparison works
                .AsEnumerable()  // 🔹 Move time filtering to memory to avoid issues
                .Where(e => e.SlotStart >= startOfficeTime && e.SlotEnd <= endOfficeTime)
                .ToList();

            // Ensure EmployeeList has a CanBook property
            foreach (var employee in Employees)
            {
                employee.Book = !string.IsNullOrEmpty(employee.Email) && employee.Email != loggedInUserEmail;
            }

            return Page();
        }


        public async Task<IActionResult> OnPostDeleteSlotAsync(int id)
        {
            Console.WriteLine($"Delete request received for ID: {id} by user {User?.Identity?.Name}");

            var employeeSlot = await _dbContext.EmployeeLists.FindAsync(id);
            if (employeeSlot == null)
            {
                Console.WriteLine("Employee slot not found.");
                return new JsonResult(new { success = false, message = "Record not found." });
            }

            if (employeeSlot.Email != User?.Identity?.Name)
            {
                Console.WriteLine("Unauthorized delete attempt.");
                return new JsonResult(new { success = false, message = "Unauthorized." });
            }

            _dbContext.EmployeeLists.Remove(employeeSlot);
            await _dbContext.SaveChangesAsync();

            Console.WriteLine("Slot deleted successfully.");
            return new JsonResult(new { success = true });
        }
    }
}
