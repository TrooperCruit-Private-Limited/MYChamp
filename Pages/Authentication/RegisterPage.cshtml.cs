using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MYChamp.Models;
using MYChamp.DbContexts;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System;

namespace MYChamp.Pages.Authentication
{
    [BindProperties]

    public class RegisterPageModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly MYChampDbContext _db;
        public RegisterModel _register { get; set; }

        public RegisterPageModel(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, MYChampDbContext dbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _db = dbContext;
            _register = new RegisterModel();
        }

      

        public IActionResult OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Index");
            }
            else
            {
                return Page();
            }

        }

        [HttpPost]
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                
                var user = _db.registerModel.FirstOrDefault(u => u.EmailId == _register.EmailId);
                if(user != null)
                {
                    ModelState.AddModelError(string.Empty, "Username Already Exists");
                    return Page();
                }

                var newUser = new AppUser
                {
                    name=_register.FirstName,
                    UserName = _register.EmailId,
                    Email = _register.EmailId,
                    address=string.Empty,
                   
                };

                var result = await _userManager.CreateAsync(newUser, _register.Password);
                if (result.Succeeded)
                {
                   
                    Random random = new Random();
                    var registerDetails = new RegisterModel
                    {
                        Id = random.Next(),
                        FirstName = _register.FirstName,
                        MiddleName = _register.MiddleName,
                        LastName = _register.LastName,
                        EmailId = _register.EmailId,
                        PhoneNumber = _register.PhoneNumber,
                        Password = _register.Password,
                        //  confirmpassword = _register.confirmpassword
                        JobTitle = _register.JobTitle,
                        CompanyName = _register.CompanyName
                    };

                    _db.registerModel.Add(registerDetails);
                    await _db.SaveChangesAsync();

                  

                    
                    return RedirectToPage("/Authentication/Login");
                }
                else
                {
                    
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
 


             

            }

            return Page();
        }
    }
}
