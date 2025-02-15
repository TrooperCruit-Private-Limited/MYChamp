using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MYChamp.DbContexts;
using MYChamp.Models;
using System.Threading.Tasks;

namespace MYChamp.Pages.Responsibilities
{
    public class OperationalModel : PageModel
    {
        private readonly MYChampDbContext _context;

        [BindProperty]
        public Responsibility Responsibility { get; set; }

        public OperationalModel(MYChampDbContext context)
        {
            _context = context;
        }

        // Handles both Create (when id is null) and Edit (when id is provided)
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)  // Create new responsibility
            {
                Responsibility = new Responsibility();
            }
            else  // Edit existing responsibility
            {
                Responsibility = await _context.Responsibilities.FindAsync(id);

                if (Responsibility == null)
                {
                    return NotFound();
                }
            }

            return Page();
        }

        // Handles both creating a new responsibility and updating an existing one
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Responsibility.Id == 0)  // Create new responsibility
            {
                _context.Responsibilities.Add(Responsibility);
            }
            else  // Update existing responsibility
            {
                var responsibilityToUpdate = await _context.Responsibilities.FindAsync(Responsibility.Id);

                if (responsibilityToUpdate == null)
                {
                    return NotFound();
                }

                // Update the fields that need to be modified
                responsibilityToUpdate.ResponsibilityName = Responsibility.ResponsibilityName;
                responsibilityToUpdate.Description = Responsibility.Description;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ResponsibilityExists(Responsibility.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("/Responsibilities/Index");
        }

        private bool ResponsibilityExists(int id)
        {
            return _context.Responsibilities.Any(e => e.Id == id);
        }
    }
}
