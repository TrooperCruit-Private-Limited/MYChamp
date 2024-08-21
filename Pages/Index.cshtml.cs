using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MYChamp.Controller;
using MYChamp.DbContexts;
using MYChamp.Models;

namespace MYChamp.Pages
{
   // [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly MYChampDbContext _db;
        private readonly SessionHandlerController _sessionHandlerController;

        public IndexModel(ILogger<IndexModel> logger, [FromServices] MYChampDbContext db,SessionHandlerController _shc)
        {
            _logger = logger;
            _db = db;
            _sessionHandlerController = _shc;
        }
        public List<VisitUsInformationModel> VisitUs {  get; set; }


        public void OnGet()
        {
            VisitUs = _db.VisitUsInformation.ToList();
            
            
        }
    }
}
