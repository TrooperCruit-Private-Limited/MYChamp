//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using Microsoft.EntityFrameworkCore;
//using MYChamp.DbContexts;
//using MYChamp.Models;

//namespace MYChamp.Pages.HolidayCalender
//{
//    public class BIndexModel : PageModel
//    {
//        private readonly MYChampDbContext _context;

//        public BIndexModel(MYChampDbContext context)
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
//                Holiday.HolidayDate = new DateTime(2025, 1, 1);
//            }
//        }

//        public async Task<IActionResult> OnGetGetHolidaysAsync()
//        {
//            try
//            {
//                var holidays = await _context.CHolidays.ToListAsync();
//                return new JsonResult(new { success = true, data = holidays });
//            }
//            catch (Exception ex)
//            {
//                return new JsonResult(new { success = false, message = ex.Message });
//            }
//        }



//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> OnPostCreateOrEditAsync([FromBody] HolidayDB holiday)
//        {
//            Console.WriteLine($"Received Data: Id={holiday.Id}, HolidayName={holiday.HolidayName}, HolidayDate={holiday.HolidayDate}, BranchName={holiday.BranchName}, Limit={holiday.Limit}");

//            // Validate input
//            if (string.IsNullOrEmpty(holiday.HolidayName))
//                return BadRequest(new { message = "HolidayName is required." });

//            if (string.IsNullOrEmpty(holiday.BranchName))
//                return BadRequest(new { message = "BranchName is required." });

//            if (holiday.Limit < 1 || holiday.Limit > 25)
//                return BadRequest(new { message = "Holiday limit must be between 1 and 25." });

//            if (holiday.HolidayDate == default)
//                return BadRequest(new { message = "HolidayDate is required." });

//            try
//            {
//                if (holiday.Id > 0)
//                {
//                    var existingHoliday = await _context.CHolidays.FindAsync(holiday.Id);
//                    if (existingHoliday != null)
//                    {
//                        // Update the existing holiday
//                        existingHoliday.HolidayName = holiday.HolidayName;
//                        existingHoliday.HolidayDate = holiday.HolidayDate.ToUniversalTime();
//                        existingHoliday.BranchName = holiday.BranchName;
//                        existingHoliday.Limit = holiday.Limit;
//                        await _context.SaveChangesAsync();
//                    }
//                    else
//                    {
//                        return BadRequest(new { success = false, message = "Holiday not found." });
//                    }
//                }
//                else
//                {
//                    // Add new holiday
//                    holiday.HolidayDate = holiday.HolidayDate.ToUniversalTime();
//                    _context.CHolidays.Add(holiday);
//                    await _context.SaveChangesAsync();
//                }

//                return new JsonResult(new { success = true });
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Exception: {ex}");
//                return new JsonResult(new { success = false, message = $"An error occurred: {ex.Message}" });
//            }
//        }










//        public async Task<IActionResult> OnPostDeleteAsync([FromBody] int id)
//        {
//            Console.WriteLine($"Received ID in delete request: {id}");  

            
//            var holiday = await _context.CHolidays.FindAsync(id);

//            if (holiday != null)
//            {
//                _context.CHolidays.Remove(holiday);
//                await _context.SaveChangesAsync();
//                return new JsonResult(new { success = true });
//            }
//            else
//            {
//                Console.WriteLine("Holiday not found in the database.");
//                return new JsonResult(new { success = false, message = "Holiday not found." });
//            }
//        }



//    }
//}
