using Microsoft.AspNetCore.Mvc.RazorPages;
using MYChamp.Models;
using MYChamp.DbContexts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MYChamp.Pages.Responsibilities
{
    public class IndexModel : PageModel
    {
        private readonly MYChampDbContext _context;

        public IndexModel(MYChampDbContext context)
        {
            _context = context;
        }

        public List<Responsibility> Responsibilities { get; set; }

        // Load the responsibilities when the page loads
        public async Task OnGetAsync()
        {
            Responsibilities = await _context.Responsibilities.ToListAsync();
        }
    }
}
