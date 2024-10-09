using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MYChamp.DbContexts;
using MYChamp.Models;

namespace MYChamp.Pages.Positions
{
    public class CreateModel : PageModel
    {
        private readonly MYChampDbContext _context;

        public CreateModel(MYChampDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Position Position { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Positions.Add(Position);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
