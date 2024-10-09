using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MYChamp.DbContexts;
using MYChamp.Models;
using System.IO;

namespace MYChamp.Pages.ArticlesF
{
    [BindProperties]
    public class EditModel : PageModel
    {
        private readonly MYChampDbContext _context;

        public Article Article { get; set; }

        public EditModel(MYChampDbContext context)
        {
            _context = context;
        }

        public void OnGet(int? id)
        {
            if (id != null && id != 0)
            {
                Article = _context.Article.Find(id);
            }
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                var existingArticle = _context.Article.AsNoTracking().FirstOrDefault(a => a.Id == Article.Id);

                if (Article.CoverPath != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        Article.CoverPath.CopyTo(memoryStream);
                        Article.CoverImageData = memoryStream.ToArray(); 
                    }
                }
                else
                {
                    Article.CoverImageData = existingArticle.CoverImageData;
                }
             
                Article.UserEmail = HttpContext.Session.GetString("username");
                _context.Attach(Article).State = EntityState.Modified;
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Article updated successfully";
                return RedirectToPage("./Index");
            }
            return Page();
        }
    }
}
