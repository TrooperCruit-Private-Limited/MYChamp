using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MYChamp.DbContexts;
using MYChamp.Models;

namespace MYChamp.Pages.HolidayCalender
{
    public class EditModel : PageModel
    {
        private readonly MYChampDbContext _context;

        [BindProperty]
        public DHolidays Holiday { get; set; }

        public EditModel(MYChampDbContext context)
        {
            _context = context;
        }

      
        public IActionResult OnGet(int id)
        {
           
            Holiday = _context.Holidays.FirstOrDefault(h => h.Id == id);
            if (Holiday == null)
            {
                return NotFound();
            }
            return Page();
        }

       
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var holidayToUpdate = _context.Holidays.FirstOrDefault(h => h.Id == Holiday.Id);
            if (holidayToUpdate == null)
            {
                return NotFound();
            }

           
            holidayToUpdate.HolidayName = Holiday.HolidayName;

            _context.SaveChanges();
            return RedirectToPage("Index");
        }
    }
}
