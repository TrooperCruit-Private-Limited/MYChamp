using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MYChamp.DbContexts;
using MYChamp.Models;

namespace MYChamp.Pages.ArticlesF
{
    [BindProperties]
    public class DetailsModel : PageModel
    {
        private readonly MYChampDbContext _context;
        public Article Article { get; set; }
        public int Rating { get; set; }
        public DetailsModel(MYChampDbContext context)
        {
            _context = context;
        }

        public void OnGet(int? id)
        {
            Article = _context.Article.Find(id);
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var article = await _context.Article.FindAsync(id);

            article.Rating = Rating;
            _context.Article.Update(article);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Rating submitted successfully!";
            return RedirectToPage("/Index");
        }

    }
}
