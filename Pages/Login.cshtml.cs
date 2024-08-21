using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MYChamp.DbContexts;
using MYChamp.Models;

namespace MYChamp.Pages
{
    public class LoginModel : PageModel
    {
        private readonly MYChampDbContext _db;
        public LoginModel(MYChampDbContext db)
        {
            _db = db;
        }
        public IActionResult OnGet()
        {
            return Page();
        }
        [HttpPost]
        public IActionResult OnPost(MyChampLogin obj)
        {
            if(ModelState.IsValid)
            {
                _db.Logins.Add(obj);
                _db.SaveChanges();
            }
            return Page();
        }
       
    }
}
