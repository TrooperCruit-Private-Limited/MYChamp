using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MYChamp.DbContexts;
using MYChamp.Models;

namespace MYChamp.Pages.Positions
{
    public class IndexModel : PageModel
    {
       
        private readonly UserManager<AppUser> _userManager;
        private readonly MYChampDbContext _db;
        public IndexModel( UserManager<AppUser> userManager,MYChampDbContext db)
        {
           
            _db = db;
            _userManager = userManager;
        }

        public IList<Position> Positions { get; set; }
        public bool IsHR{  get; set; }
        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            //var employee = _context.Employees
            //               .Include(e => e.Position)
            //               .FirstOrDefault(e => e.Email == user.Email);
            var data=_db.registerModel.FirstOrDefault(e => e.EmailId == user.Email);
            IsHR = data != null && data.JobTitle == "HR";

            Positions = _db.Positions.Where(a=>!a.IsActiveP).ToList();
        }

        public async Task<IActionResult> OnPostArchiveAsync(int id)
        {
            var position=await _db.Positions.FirstOrDefaultAsync(e => e.Id == id);
            if (position != null) {
                position.IsActiveP = true;
                _db.Update(position);
                await _db.SaveChangesAsync();
            }
            return RedirectToPage();
        }
    }
}
