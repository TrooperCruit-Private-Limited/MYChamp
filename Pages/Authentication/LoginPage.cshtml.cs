using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MYChamp.Controller;
using MYChamp.DbContexts;
using MYChamp.Models;

using System;
using System.Threading.Tasks;

namespace MYChamp.Pages.Authentication
{
    [AllowAnonymous]
    public class LoginPageModel : PageModel
    {
        [BindProperty]
        public LoginModel login_Model { get; set; }

        private readonly SignInManager<AppUser> _signInManager;
        private readonly MYChampDbContext _db;
      //  private readonly LeaveManagementDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        public LoginController loginController;

        public LoginPageModel(MYChampDbContext db, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager,LoginController lc)
        {
            _db = db;
            _signInManager = signInManager;
            _userManager = userManager;
            loginController = lc;
        //    _context = context;
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
                //else if (resultValue == 2) // Successful login
                //{
                //    var user = await _userManager.FindByEmailAsync(login_Model.Name);
                //    var isManager = _context.Employees.Any(e => e.Email == user.Email && e.Position.Name == "Manager");

                //    if (isManager)
                //    {
                //        return RedirectToPage("/Manager/ReviewRequests");
                //    }
                //    else
                //    {
                //        return RedirectToPage("/Index");
                //    }
                //}

            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            return Page();
        }

    }
}
