using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MYChamp.Controller;
using MYChamp.DbContexts;
using MYChamp.Models;
using System.Security.Claims;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MYChamp.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly MYChampDbContext _db;
        private readonly UserManager<AppUser> _userManager;
        private readonly SessionHandlerController _sessionHandlerController;

        public IndexModel(
            ILogger<IndexModel> logger,
            [FromServices] MYChampDbContext db,
            UserManager<AppUser> userManager,
            SessionHandlerController sessionHandlerController)
        {
            _logger = logger;
            _db = db;
            _userManager = userManager;
            _sessionHandlerController = sessionHandlerController;
        }

        public List<VisitUsInformationModel> VisitUs { get; set; }
        public List<Article> ArticleList { get; set; }
        public List<string> Categories { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SelectedCategory { get; set; }
        public string UserResponsibilityNames { get; set; }
        public List<MenuItem> MenuItemsList { get; set; }

        public async Task OnGetAsync()
        {
            // Fetch VisitUs information
            VisitUs = await _db.VisitUsInformation.ToListAsync();

            // Fetch distinct categories for articles
            Categories = await _db.Article
                .Where(a => !a.IsArchived)
                .Select(a => a.Category)
                .Distinct()
                .ToListAsync();

            // Fetch articles based on the selected category
            if (!string.IsNullOrEmpty(SelectedCategory))
            {
                ArticleList = await _db.Article
                    .Where(a => a.Category == SelectedCategory && !a.IsArchived)
                    .OrderByDescending(a => a.Id)
                    .ToListAsync();
            }
            else
            {
                ArticleList = await _db.Article
                    .Where(a => !a.IsArchived)
                    .OrderByDescending(a => a.Id)
                    .ToListAsync();
            }

            // Fetch the logged-in user's email
            var user = await _userManager.GetUserAsync(User);
            var email = user?.Email;

            if (!string.IsNullOrEmpty(email))
            {
                // Retrieve the employee record based on email
                var employee = await _db.Employees
                    .FirstOrDefaultAsync(e => e.Email == email);

                if (employee != null && employee.SelectedResponsibilityIds.Any())
                {
                    // Fetch responsibility names based on SelectedResponsibilityIds
                    var menuItems = await _db.Responsibilities
                        .Where(r => employee.SelectedResponsibilityIds.Contains(r.Id))
                       .SelectMany(r => r.MenuItems) 
                        .Distinct() 
                        .ToListAsync();

                    MenuItemsList = menuItems;

                    var responsibilities = await _db.Responsibilities
                        .Where(r => employee.SelectedResponsibilityIds.Contains(r.Id))
                        .Select(r => r.Description)
                        .ToListAsync();

                    // Join responsibility names with commas
                    UserResponsibilityNames = string.Join(", ", responsibilities);
                }
                else
                {
                    UserResponsibilityNames = string.Empty;
                    MenuItemsList = new List<MenuItem>();
                }
            }
            else
            {
                UserResponsibilityNames = string.Empty;
                MenuItemsList = new List<MenuItem>();
            }
        }
    }
}
