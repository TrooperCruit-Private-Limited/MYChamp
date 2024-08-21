using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MYChamp.AuthModel;
using MYChamp.Controller;
using MYChamp.Models;


namespace MYChamp.Pages.Auth
{
    
    public class Forceful_logoutModel : PageModel
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly SessionHandlerController _sessionHandlerController;
       

        public Forceful_logoutModel(SignInManager<AppUser> signInManager, SessionHandlerController _shc)
        {
            _signInManager = signInManager;
            _sessionHandlerController= _shc;

        }
        public async Task<IActionResult> OnGetAsync()
        {
            var sessionId = HttpContext.Session.Id;
            var ipAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var username = HttpContext.Session.GetString("username");
            _sessionHandlerController.UpdateForceLogout(username);
            var password= HttpContext.Session.GetString("password");
            var result = await _signInManager.PasswordSignInAsync(username, password, true, false);
            string loginId = HttpContext.Session.GetString("loginId");

           

            if (result.Succeeded)
            {
                var loginTime = DateTime.UtcNow;
                _sessionHandlerController.AddSessionInformation(sessionId,username, ipAddress, loginTime,"loginId");
                bool value= _sessionHandlerController.check(HttpContext.Session.GetString("uniqueid"));
                Console.WriteLine("some value " + HttpContext.Session.GetString("uniqueid"));
               Console.WriteLine(value+" boolean value");
                if (value)
                {
                    await _signInManager.SignOutAsync();
                }
                return RedirectToPage("/Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return Page();
            }

            // _sessionService.AddSessionInformation(TempData["username"], )


            
        }
    }
}
