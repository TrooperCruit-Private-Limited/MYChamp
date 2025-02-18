//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using Microsoft.EntityFrameworkCore;
//using MYChamp.DbContexts;
//using MYChamp.Models;
//using System.Text.Json;

//namespace MYChamp.Pages.CHolidayCalender
//{
//    public class IndexModel : PageModel
//    {
//        private readonly MYChampDbContext _context;

//        public IndexModel(MYChampDbContext context)
//        {
//            _context = context;
//        }


//        [BindProperty]
//        public HolidayDB Holiday { get; set; } = new HolidayDB(); 

//        public List<HolidayDB> Holidays { get; set; } = new List<HolidayDB>(); 

//        public async Task OnGetAsync()
//        {
            
//            Holidays = await _context.CHolidays.ToListAsync();
//            if (Holiday.HolidayDate == default)
//            {
//                // Holiday.HolidayDate = new DateTime(2025, 1, 1); 
//                Holiday.HolidayDate = new DateTimeOffset(new DateTime(2025, 1, 1), TimeSpan.Zero);
//            }
//        }


//        public async Task<IActionResult> OnPostCreateOrEditAsync()
//        {
//            if (!ModelState.IsValid)
//            {
//                await LoadHolidaysAsync();
//                return Page();
//            }

//            if (Holiday.Id > 0)
//            {
                
//                var existingHoliday = await _context.CHolidays.FindAsync(Holiday.Id);
//                if (existingHoliday != null)
//                {
//                    existingHoliday.HolidayName = Holiday.HolidayName;
//                    // existingHoliday.HolidayDate = Holiday.HolidayDate.UtcDateTime;
//                    existingHoliday.HolidayDate = new DateTimeOffset(Holiday.HolidayDate.Date, TimeSpan.Zero);

//                    existingHoliday.BranchName = Holiday.BranchName;
//                    existingHoliday.Limit = Holiday.Limit;
//                    await _context.SaveChangesAsync();
//                }
//            }
//            else
//            {

//                // Holiday.HolidayDate = Holiday.HolidayDate.UtcDateTime;
//                Holiday.HolidayDate = new DateTimeOffset(Holiday.HolidayDate.Date, TimeSpan.Zero);
//                _context.CHolidays.Add(Holiday);
//                await _context.SaveChangesAsync();
//            }

//            return RedirectToPage();
//        }

//        public async Task<IActionResult> OnPostDeleteAsync(int id)
//        {
//            var holiday = await _context.CHolidays.FindAsync(id);
//            if (holiday != null)
//            {
//                _context.CHolidays.Remove(holiday);
//                await _context.SaveChangesAsync();
//            }

//            return RedirectToPage();
//        }

//        private async Task LoadHolidaysAsync()
//        {
//            Holidays = await _context.CHolidays.ToListAsync();
//        }

        





//    }
//}
