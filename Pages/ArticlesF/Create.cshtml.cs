using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MYChamp.DbContexts;
using MYChamp.Models;

namespace MYChamp.Pages.ArticlesF
{
    [BindProperties]
    public class CreateModel : PageModel
    {
        private readonly MYChampDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public Article Article { get; set; }

        public CreateModel(MYChampDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public void OnGet(int? id)
        {
          if (id == null)
          { 
            Article = new Article();
            Article.UserEmail = HttpContext.Session.GetString("username");
          }
            else
            {
                Article = _context.Article.Find(id);
            }
    }
        public async Task<IActionResult> OnPostAsync()
        {
            var existingArticle = _context.Article.AsNoTracking().FirstOrDefault(a => a.Id == Article.Id);
            if (Article.CoverPath != null)
            {
                using (var memoryStream = new MemoryStream())
                {

                    await Article.CoverPath.CopyToAsync(memoryStream);
                    Article.CoverImageData = memoryStream.ToArray();

                }
            }
            else
            {
                Article.CoverImageData = existingArticle.CoverImageData;
            }
            if (ModelState.IsValid)
            {
                //  Article.UserEmail = HttpContext.Session.GetString("username");
                if (Article.Id == 0)
                {
                    _context.Article.Add(Article);
                }
                else
                {
                    _context.Attach(Article).State = EntityState.Modified;

                }
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Article added successfully.";
                return RedirectToPage("Index");
            }

            return Page();
        }
    }
}
