using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MYChamp.DbContexts;
using MYChamp.Models;

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

       


        public List<CDHioliday> Holidays { get; set; } = new List<CDHioliday>(); 

        public void OnGet()
        {
           
            int currentYear = DateTime.Now.Year;
            string defaultCountry = "India";

            InsertDefaultHolidays(new List<int> { 2026, 2027, 2028, 2029 });

            Holidays = GetHolidaysForYearAndCountry(currentYear, defaultCountry);
        }

        private void InsertDefaultHolidays(List<int> years)
        {
            var existingHolidayCount = _context.Holiday.Count(); 
            if (existingHolidayCount > 0) return; 

            var holidays = new List<CDHioliday>();

            foreach (var year in years)
            {
                holidays.Add(new CDHioliday { Date = new DateTime(year, 1, 1), Name = "New Year's Day", Country = "India" });
                holidays.Add(new CDHioliday { Date = new DateTime(year, 1, 26), Name = "Republic Day", Country = "India" });
                holidays.Add(new CDHioliday { Date = new DateTime(year, 8, 15), Name = "Independence Day", Country = "India" });
            }

            _context.Holiday.AddRange(holidays);
            _context.SaveChanges(); 
        }


        public JsonResult OnGetFetchHolidays(int year, string country)
        {
            if (string.IsNullOrWhiteSpace(country) || year <= 0)
            {
                Console.WriteLine("Invalid year or country input.");
                return new JsonResult(new List<CDHioliday>());
            }

            var holidays = GetHolidaysForYearAndCountry(year, country);
            Console.WriteLine($"Fetched {holidays.Count} holidays for year {year} and country {country}");
            return new JsonResult(holidays);
        }



       

        private List<CDHioliday> GetHolidaysForYearAndCountry(int year, string country)
        {
            return _context.Holiday
                .Where(h => h.Date.Year == year && h.Country == country)
                .ToList();
        }


    }
}
