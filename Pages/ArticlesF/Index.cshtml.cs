
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MYChamp.DbContexts;
using MYChamp.Models;
using System.Collections.Generic;
using System.Linq;

namespace MYChamp.Pages.ArticlesF
{
    public class IndexModel : PageModel
    {
        private readonly MYChampDbContext _context;

        public IndexModel(MYChampDbContext context) 
        {
            _context = context;
        }

        public IList<Article> ArticlesList { get; set; }
        public List<string> Categories { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SelectedCategory { get; set; }

        public void OnGet()
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value;
            Categories = _context.Article
                .Where(a => a.UserEmail == userEmail && !a.IsArchived)
                .Select(a => a.Category)
                .Distinct()
                .ToList();

            if (!string.IsNullOrEmpty(SelectedCategory))
            {
                ArticlesList = _context.Article
                    .Where(a => a.UserEmail == userEmail && !a.IsArchived && a.Category == SelectedCategory)
                    .ToList();
            }
            else
            {
                ArticlesList = _context.Article
                    .Where(a => a.UserEmail == userEmail && !a.IsArchived)
                    .ToList();
            }
        }

        public IActionResult OnPostArchive(int id)
        {
            var article = _context.Article.Find(id);
            if (article != null)
            {
                article.IsArchived = true;
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Article archived";
            }
            return RedirectToPage();
        }
    }
}
