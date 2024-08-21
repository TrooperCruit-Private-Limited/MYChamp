using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MYChamp.AuthModel;
using MYChamp.Controller;
using MYChamp.DbContexts;
using MYChamp.Models;

using System;
using System.Threading.Tasks;

namespace MYChamp.Pages.Auth
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        [BindProperty]
        public Login_model login_Model { get; set; }

        private readonly SignInManager<AppUser> _signInManager;
        private readonly MYChampDbContext _db;
        private readonly UserManager<AppUser> _userManager;
        public LoginController loginController;

        public LoginModel(MYChampDbContext db, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager,LoginController lc)
        {
            _db = db;
            _signInManager = signInManager;
            _userManager = userManager;
            loginController = lc;
        }

        public IActionResult OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Index");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            foreach (var modelError in ViewData.ModelState.Values.SelectMany(v => v.Errors))
            {
             Console.Write(modelError.ErrorMessage);
                
            }
            
            if (ModelState.IsValid) // Validate login form data
            {
                ActionResult<int> loginResult = await loginController.OnPostAsync(login_Model);
                int resultValue = loginResult.Value;
                if (resultValue == 1 || resultValue == 3 || resultValue == 4)
                {
                    return Page();
                }
                else if (resultValue == 2)
                {
                    return RedirectToPage("/Index");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            return Page();
        }

    }
}
