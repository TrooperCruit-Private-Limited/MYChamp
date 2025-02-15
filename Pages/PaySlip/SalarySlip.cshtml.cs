using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MYChamp.DbContexts;
using Microsoft.EntityFrameworkCore;
using MYChamp.Models;
using Microsoft.AspNetCore.Identity;

namespace MYChamp.Pages.PaySlip
{
    public class SalarySlipModel : PageModel
    {
        private readonly MYChampDbContext _db;
        private readonly UserManager<AppUser> _userManager;

        public SalarySlipModel(MYChampDbContext db, UserManager<AppUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public List<SalaryDispatch> SalaryDispatchRecords { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                // Get the logged-in user
                var user = await _userManager.GetUserAsync(User);
                var email = user?.Email;

                // If email is null or user is not authenticated, redirect to the login page
                if (string.IsNullOrEmpty(email))
                {
                    return RedirectToPage("/Authentication/LoginPage");
                }

                // Fetch the salary dispatch record associated with the user's email
                SalaryDispatchRecords = await _db.SalaryDispatches
                    .Where(s => s.Email == email)
                    .ToListAsync();

                // If no record is found, display an error message
                if (SalaryDispatchRecords == null)
                {
                    TempData["ErrorMessage"] = "No salary records found for your account.";
                    return Page();
                }
            }
            catch (Exception ex)
            {
                // Log the exception and display a user-friendly message
                TempData["ErrorMessage"] = "An error occurred while retrieving your salary details. Please try again later.";
                Console.WriteLine($"Error: {ex.Message}");
                return Page();
            }

            return Page();
        }
    }
}
