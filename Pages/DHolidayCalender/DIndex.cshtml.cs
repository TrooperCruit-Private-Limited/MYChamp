using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MYChamp.DbContexts;
using MYChamp.Models;
using Newtonsoft.Json;
using System.Text.Json;

namespace MYChamp.Pages.DHolidayCalender
{
    public class DIndexModel : PageModel
    {


        private readonly MYChampDbContext _context;

        public DIndexModel(MYChampDbContext context)
        {
            _context = context;
        }
        public List<string> Countries { get; set; } = new List<string>
    {
        "Australia", "Thailand", "Philippines", "Chile", "Peru", "Ecuador", "Malaysia", "India", "Uruguay"
    };

       


        public List<CDHoliday> Holidays { get; set; } = new List<CDHoliday>(); 

        public void OnGet()
        {
           
            int currentYear = DateTime.Now.Year;
            string defaultCountry = "India";

            InsertDefaultHolidays(new List<int> {2025, 2026, 2027, 2028, 2029 });

            Holidays = GetHolidaysForYearAndCountry(currentYear, defaultCountry);
        }

        private void InsertDefaultHolidays(List<int> years)
        {
            var existingHolidayCount = _context.Holiday.Count(); 
            if (existingHolidayCount > 0) return; 

            var holidays = new List<CDHoliday>();

            foreach (var year in years)
            {
                holidays.Add(new CDHoliday { Date = new DateTime(year, 1, 1), Name = "New Year's Day", Country = "India" });
                holidays.Add(new CDHoliday { Date = new DateTime(year, 1, 26), Name = "Republic Day", Country = "India" });
                holidays.Add(new CDHoliday { Date = new DateTime(year, 8, 15), Name = "Independence Day", Country = "India" });
            }

            _context.Holiday.AddRange(holidays);
            _context.SaveChanges(); 
        }


        public JsonResult OnGetFetchHolidays(int year, string country)
        {
            if (string.IsNullOrWhiteSpace(country) || year <= 0)
            {
                Console.WriteLine("Invalid year or country input.");
                return new JsonResult(new List<CDHoliday>());
            }

            var holidays = GetHolidaysForYearAndCountry(year, country);
            Console.WriteLine($"Fetched {holidays.Count} holidays for year {year} and country {country}");
            return new JsonResult(holidays);
        }



       

        private List<CDHoliday> GetHolidaysForYearAndCountry(int year, string country)
        {
            return _context.Holiday
                .Where(h => h.Date.Year == year && h.Country == country)
                .ToList();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult OnPostAddHoliday([FromBody] List<CDHoliday> holidays)
        {
            if (holidays == null || !holidays.Any())
            {
                return BadRequest(new { success = false, message = "No holiday data received" });
            }

            try
            {
                
                foreach (var holiday in holidays)
                {
                    holiday.Date = DateTime.SpecifyKind(holiday.Date, DateTimeKind.Utc);
                }

                _context.Holiday.AddRange(holidays);
                int recordsSaved = _context.SaveChanges();
                return new JsonResult(new { success = true, message = "Holidays added successfully!" });
            }
            catch (DbUpdateException dbEx)
            {
                
                Console.WriteLine($"❌ Database update error: {dbEx.InnerException?.Message ?? dbEx.Message}");
                return StatusCode(500, new { success = false, message = "Database error", error = dbEx.InnerException?.Message ?? dbEx.Message });
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"❌ An unexpected error occurred: {ex.Message}");
                return StatusCode(500, new { success = false, message = "An unexpected error occurred", error = ex.Message });
            }
        }

        public async Task<IActionResult> OnPostDeleteHoliday([FromBody] JsonElement jsonData)
        {
            try
            {
               
                string dateString = jsonData.GetProperty("Date").GetString();
                string country = jsonData.GetProperty("Country").GetString();
                string name = jsonData.GetProperty("Name").GetString();

                if (string.IsNullOrEmpty(dateString) || string.IsNullOrEmpty(country) || string.IsNullOrEmpty(name))
                {
                    Console.WriteLine($"❌ Invalid request received! Missing data.");
                    return BadRequest(new { success = false, message = "Invalid holiday data. Date, Country, and Name are required!" });
                }

                DateTime holidayDate;
                if (!DateTime.TryParse(dateString, out holidayDate))
                {
                    Console.WriteLine("❌ Error: Invalid Date format received.");
                    return BadRequest(new { success = false, message = "Invalid date format!" });
                }

                holidayDate = DateTime.SpecifyKind(holidayDate.Date, DateTimeKind.Utc);

                Console.WriteLine($"🔍 Debug: Deleting Holiday - Date: {holidayDate}, Country: {country}, Name: {name}");

                var holidayToDelete = await _context.Holiday
                    .FirstOrDefaultAsync(h => h.Date.Date == holidayDate.Date && h.Country == country && h.Name == name);

                if (holidayToDelete != null)
                {
                    _context.Holiday.Remove(holidayToDelete);
                    await _context.SaveChangesAsync();
                    Console.WriteLine("✅ Holiday deleted successfully!");
                    return new JsonResult(new { success = true, message = "Holiday deleted successfully" });
                }

                Console.WriteLine("❌ Holiday not found in the database!");
                return new JsonResult(new { success = false, message = "Holiday not found in the database!" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error deleting holiday: {ex.Message}");
                return StatusCode(500, new { success = false, message = "An error occurred while deleting the holiday.", error = ex.Message });
            }
        }




    }
}
