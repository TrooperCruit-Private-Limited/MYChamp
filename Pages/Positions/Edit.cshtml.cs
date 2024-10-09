using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MYChamp.DbContexts;
using MYChamp.Models;

namespace MYChamp.Pages.Positions
{
    public class EditModel : PageModel
    {
        private readonly MYChampDbContext _context;

        public EditModel(MYChampDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Position Position { get; set; }

        public IActionResult OnGet(int? id)
        {
            Position = _context.Positions.Find(id);
            return Page();
        }

        //public IActionResult OnPost()
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return Page();
        //    }
        //    _context.Update(Position);
        //    _context.SaveChanges();
        //    return RedirectToPage("./Index");
        //}        

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
               return Page();
            }
          
            var positionToUpdate = _context.Positions.AsNoTracking().FirstOrDefault(p => p.Id == Position.Id);
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
            
            _context.Update(Position);
            _context.SaveChanges();

            return RedirectToPage("./Index");
        }

    }
}    
