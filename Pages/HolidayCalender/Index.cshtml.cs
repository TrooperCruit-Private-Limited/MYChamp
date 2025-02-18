using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MYChamp.DbContexts;
using MYChamp.Models;
using System.ComponentModel.DataAnnotations;

namespace MYChamp.Pages.HolidayCalender
{
    public class IndexModel : PageModel
    {
        
        private readonly MYChampDbContext _context;

        public IndexModel(MYChampDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public List<HolidayInputModel> Holidays { get; set; } = new List<HolidayInputModel>();

        public List<DHolidays> SavedHolidays { get; set; } = new List<DHolidays>();

        [BindProperty]
        public int? EditHolidayId { get; set; }
        public class HolidayInputModel
        {
            [Required]
            [DataType(DataType.Date)]
            [Range(typeof(DateTime), "2024-01-01", "2099-12-31", ErrorMessage = "Date must be in the future.")]
            public DateTime HolidayDate { get; set; }

            [Required]
            [MaxLength(100)]
            public string HolidayName { get; set; }

            

        }

        public void OnGet()
        {

            SavedHolidays = _context.Holidays.OrderBy(h => h.HolidayDate).ToList();
            if (Holidays == null || !Holidays.Any())
            {
                Holidays.Add(new HolidayInputModel());
            }

            //SavedHolidays = _context.Holidays.OrderBy(h => h.HolidayDate).ToList();
        }




        //public async Task<IActionResult> OnPostAsync()
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return Page();
        //    }




        //    var submittedDates = Holidays.Select(h => h.HolidayDate.Date).ToList();
        //    var existingHolidays = _context.Holidays
        //                                   .Where(h => submittedDates.Contains(h.HolidayDate.Date))
        //                                   .Select(h => h.HolidayDate.Date)
        //                                   .ToList();


        //    if (existingHolidays.Any())
        //    {
        //        return new JsonResult(new
        //        {
        //            success = false,
        //            message = "Duplicate dates found.",
        //            duplicates = existingHolidays.Select(d => d.ToString("yyyy-MM-dd"))
        //        });
        //    }

        //    var uniqueHolidays = Holidays
        //                         .Where(h => !existingHolidays.Contains(h.HolidayDate.Date))
        //                         .Select(h => new Holiday
        //                         {
        //                             HolidayDate = DateTime.SpecifyKind(h.HolidayDate, DateTimeKind.Utc),
        //                             HolidayName = h.HolidayName
        //                         }).ToList();

        //    if (uniqueHolidays.Any())
        //    {
        //        _context.Holidays.AddRange(uniqueHolidays);
        //        await _context.SaveChangesAsync();
        //    }

        //    SavedHolidays = _context.Holidays.OrderBy(h => h.HolidayDate).ToList();

        //    return new JsonResult(new
        //    {
        //        success = true,
        //        added = uniqueHolidays.Select(h => new
        //        {
        //            holidayDate = h.HolidayDate.ToLocalTime().ToString("yyyy-MM-dd"),
        //            holidayName = h.HolidayName
        //        })
        //    });
        //}


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            
            var submittedDates = Holidays.Select(h => h.HolidayDate.Date).ToList();
            var existingHolidays = _context.Holidays
                                           .Where(h => submittedDates.Contains(h.HolidayDate.Date))
                                           .Select(h => h.HolidayDate.Date)
                                           .ToList();

            
            var uniqueHolidays = Holidays
                                 .Where(h => !existingHolidays.Contains(h.HolidayDate.Date))
                                 .Select(h => new DHolidays
                                 {
                                     HolidayDate = DateTime.SpecifyKind(h.HolidayDate, DateTimeKind.Utc),
                                     HolidayName = h.HolidayName
                                 }).ToList();

            if (uniqueHolidays.Any())
            {
                _context.Holidays.AddRange(uniqueHolidays);
                await _context.SaveChangesAsync();
            }

            SavedHolidays = _context.Holidays.OrderBy(h => h.HolidayDate).ToList();

            return new JsonResult(new
            {
                success = true,
                duplicates = existingHolidays.Select(d => d.ToString("yyyy-MM-dd")),
                added = uniqueHolidays.Select(h => new
                {
                    holidayDate = h.HolidayDate.ToLocalTime().ToString("yyyy-MM-dd"),
                    holidayName = h.HolidayName
                })
            });
        }




        public async Task<IActionResult> OnPostDeleteHolidayAsync([FromBody] DeleteHolidayRequest request)
        {
            var holiday = await _context.Holidays.FindAsync(request.HolidayId);

            if (holiday == null)
            {
                return new JsonResult(new { success = false, message = "Holiday not found." });
            }

            _context.Holidays.Remove(holiday);
            await _context.SaveChangesAsync();

            SavedHolidays = _context.Holidays.OrderBy(h => h.HolidayDate).ToList();

            return new JsonResult(new { success = true, message = "Holiday deleted successfully." });
        }

        public class DeleteHolidayRequest
        {
            public int HolidayId { get; set; }
        }




    }
}
