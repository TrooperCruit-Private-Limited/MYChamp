using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MYChamp.Controller;
using MYChamp.Models;


namespace MYChamp.Pages.Auth
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly SessionHandlerController sessionHandlerController;
    

        public LogoutModel(SignInManager<AppUser> signInManager,SessionHandlerController shc)
        {
            _signInManager = signInManager;
            sessionHandlerController = shc;
              
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var sessionId = HttpContext.Session.Id;
            var username = User.Identity.Name;
            Console.WriteLine("username" + username);
            sessionHandlerController.UpdateSessionInformation(username);
            await _signInManager.SignOutAsync();
            HttpContext.Session.Clear();    
            return RedirectToPage("/Index");
        }
    }
}
