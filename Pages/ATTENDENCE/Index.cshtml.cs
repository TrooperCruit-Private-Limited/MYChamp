
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MYChamp.Controller;
using MYChamp.DbContexts;
using MYChamp.Migrations;
using MYChamp.Models;

namespace MYChamp.Pages.ATTENDENCE
{
    public class IndexModel : PageModel
    {
        private readonly MYChampDbContext _context;

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private object lastPunch;

        public IndexModel(MYChampDbContext context, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
           
        }

        [BindProperty]
        public AttendenceModel Attendence { get; set; } = default;



        public async Task<IActionResult> OnGetAsync()
        
        {    

            string UserID = _httpContextAccessor.HttpContext.Session.GetString("loginId");
            Attendence = new AttendenceModel();
            Attendence.UserID =Convert.ToInt32(UserID);                          

           return Page();
        }
        
        public async Task<IActionResult> OnPostAsync()
        {
          
            if (!ModelState.IsValid)
            {
                return Page();
            }

           
            
            Attendence.DateTime = DateTime.UtcNow;
            _context.Attendence.Add(Attendence);
            await _context.SaveChangesAsync();
            

            return RedirectToPage("Success");
        }
    }    
    
}

