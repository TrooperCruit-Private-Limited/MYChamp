using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MYChamp.DbContexts;
using MYChamp.Models;
using System.Linq;

namespace MYChamp.Pages.ArticlesF
{
    [BindProperties]
    public class DeleteModel : PageModel
    {
        private readonly MYChampDbContext _context;

        public Article Article { get; set; }

        public DeleteModel(MYChampDbContext context)
        {
            _context = context;
        }

        public void OnGet(int? id)
        {
            if (id.HasValue && id.Value != 0)
            {
                Article = _context.Article.FirstOrDefault(a => a.Id == id.Value);
                if (Article == null)
                {
                    TempData["ErrorMessage"] = "Article not found.";
                    RedirectToPage("./Index");
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Invalid Article ID.";
                RedirectToPage("./Index");
            }
        }

        public IActionResult OnPost()
        {
            if (Article?.Id != null)
            {
                var articleToDelete = _context.Article.FirstOrDefault(a => a.Id == Article.Id);

                if (articleToDelete != null)
                {
                    _context.Article.Remove(articleToDelete);
                    _context.SaveChanges();
                    TempData["SuccessMessage"] = "Article deleted successfully";
                }
                else
                {
                    TempData["ErrorMessage"] = "Article not found for deletion.";
                }
            }

            return RedirectToPage("./Index");
        }
    }
}
