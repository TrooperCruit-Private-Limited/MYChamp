using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MYChamp.DbContexts;
using MYChamp.Models;

namespace MYChamp.Pages.Positions
{
    public class ArchivedModel : PageModel
    {
      
        private readonly UserManager<AppUser> _userManager;
        private readonly MYChampDbContext _db;
        public ArchivedModel( UserManager<AppUser> userManager, MYChampDbContext db)
        {
           
            _db = db;
            _userManager = userManager;
        }

        public IList<Position> Positions { get; set; }

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var data = _db.registerModel.FirstOrDefault(e => e.EmailId == user.Email);
            if (data != null && data.JobTitle == "HR")
            {
                Positions = await _db.Positions.Where(a => a.IsActiveP).ToListAsync();
            }
        }

        public async Task<IActionResult> OnPostUnArchiveAsync(int id)
        {
            var position = await _db.Positions.FirstOrDefaultAsync(e => e.Id == id);
            if (position != null)
            {
                position.IsActiveP = false;
                _db.Update(position);
                await _db.SaveChangesAsync();
            }
            return RedirectToPage();
        }
    }
}
