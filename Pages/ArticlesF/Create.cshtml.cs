using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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

        public void OnGet()
        {
            Article = new Article();
            Article.UserEmail = HttpContext.Session.GetString("username");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            using (var memoryStream = new MemoryStream())
            {

                await Article.CoverPath.CopyToAsync(memoryStream);
                Article.CoverImageData = memoryStream.ToArray();

            }
            if (ModelState.IsValid)
            { 
                Article.UserEmail = HttpContext.Session.GetString("username");

                _context.Article.Add(Article);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Article added successfully.";
                return RedirectToPage("Index");
            }

            return Page();
        }
    }
}
