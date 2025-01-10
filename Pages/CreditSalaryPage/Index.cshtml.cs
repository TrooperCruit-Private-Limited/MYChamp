using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MYChamp.DbContexts;
using MYChamp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MYChamp.Pages.CreditSalaryPage
{
    public class IndexModel : PageModel
    {
        private readonly MYChampDbContext _db;

        public IndexModel(MYChampDbContext db)
        {
            _db = db;
        }

        public List<SalaryDispatch> SalaryDispatchRecords { get; set; } = new();

        public async Task OnGetAsync()
        {
            // Fetch SalaryDispatch records from the database
            SalaryDispatchRecords = await _db.SalaryDispatches.ToListAsync();
        }
    }
}
