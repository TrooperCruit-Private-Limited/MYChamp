using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MYChamp.DbContexts;
using MYChamp.Models;

namespace MYChamp.Pages.ArticlesF
{
    public class ArchivedModel : PageModel
    {
        private readonly MYChampDbContext _context;

        public ArchivedModel(MYChampDbContext context)
        {
            _context = context;
        }

        public IList<Article> ArchivedArticles { get; set; }

        public void OnGet()
        {
            var EmailUser = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value;
            ArchivedArticles = _context.Article
                .Where(a => a.UserEmail == EmailUser && a.IsArchived)
                .ToList();
        }
        public IActionResult OnPostUnarchive(int id)
        {
            var article = _context.Article.Find(id);
            if (article != null)
            {
                article.IsArchived = false;
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Article unarchived";
            }
            return RedirectToPage("index");
        }
    }
}
