using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
               
                Position = new Position();
            }
            else
            {
               
                Position = await _context.Positions.FindAsync(id);

                if (Position == null)
                {
                    return NotFound();
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Position.Id == 0)
            {
                _context.Positions.Add(Position);
            }
            else
            {
               
                var positionToUpdate = await _context.Positions
                                                      .AsNoTracking()
                                                      .FirstOrDefaultAsync(p => p.Id == Position.Id);
                if (positionToUpdate == null)
                {
                    return NotFound();
                }

                int differenceInLeaves = Position.AllottedLeaves - positionToUpdate.AllottedLeaves;

                if (differenceInLeaves != 0)
                {
                    var employees = _context.Employees
                                            .Where(e => e.PositionId == Position.Id && !e.IsActive)
                                            .ToList();

                    foreach (var employee in employees)
                    {
                        employee.RemainingLeaves += differenceInLeaves;

                        if (employee.RemainingLeaves < 0)
                        {
                            employee.RemainingLeaves = 0;
                        }

                        _context.Entry(employee).State = EntityState.Modified;
                    }
                }

                _context.Positions.Update(Position);
            }

            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
