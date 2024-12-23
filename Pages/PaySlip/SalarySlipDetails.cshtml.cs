using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MYChamp.DbContexts;
using MYChamp.Models;
using Microsoft.EntityFrameworkCore;
namespace MYChamp.Pages.PaySlip
{
    public class SalarySlipDetailsModel : PageModel
    {
        private readonly MYChampDbContext _db;

        public SalarySlipDetailsModel(MYChampDbContext db)
        {
            _db = db;
        }

        public SalaryDispatch SalaryDispatch { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                // Retrieve the specific salary dispatch record by ID
                SalaryDispatch = await _db.SalaryDispatches.FirstOrDefaultAsync(s => s.Id == id);

                // Check if the record exists
                if (SalaryDispatch == null)
                {
                    TempData["ErrorMessage"] = "Salary record not found.";
                    return RedirectToPage("/PaySlip/SalarySlip");
                }
            }
            catch (Exception ex)
            {
                // Log the error and show an error message
                TempData["ErrorMessage"] = "An error occurred while retrieving salary details.";
                Console.WriteLine($"Error: {ex.Message}");
                return RedirectToPage("/PaySlip/SalarySlip");
            }

            return Page();
        }

    }
}
