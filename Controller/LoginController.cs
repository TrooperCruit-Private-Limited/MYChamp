using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MYChamp.AuthModel;
using MYChamp.DbContexts;
using MYChamp.Models;

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Net.Http;

namespace MYChamp.Controller
{
    [AllowAnonymous]
    public class LoginController : ControllerBase
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly MYChampDbContext _db;
        private readonly SessionHandlerController _sessionHandlerController;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public LoginController(MYChampDbContext db, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, SessionHandlerController sessionHandlerController, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _db = db;
            _signInManager = signInManager;
            _userManager = userManager;
            _sessionHandlerController = sessionHandlerController;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost]
        public async Task<ActionResult<int>> OnPostAsync(Login_model login_Model)
        {
            var sequenceId = _httpContextAccessor.HttpContext.Session.GetString("sequence");  // Access session value
            _httpContextAccessor.HttpContext.Session.SetString("sequence", Guid.NewGuid().ToString()); // Set session value

            if (ModelState.IsValid)
            {

                var user = _db.signinaccounts.FirstOrDefault(u=>u.EmailId == login_Model.Name
                 );
                
                if (user != null &&  (user.Password==login_Model.Password))
                {
                    /* storing the login information in static class */
                    MyChampIdentity myChampIdentity = new MyChampIdentity();
                    myChampIdentity.username = user.EmailId;
                    myChampIdentity.LoginId=user.Id ;
                    _httpContextAccessor.HttpContext.Session.SetString("loginId", user.Id.ToString()
                        );
                    var existingSession = _sessionHandlerController.SessionExists(user.EmailId.ToString());
                    Console.WriteLine(existingSession + "  " + login_Model.Name);
                     
                    if (existingSession)
                    {
                        myChampIdentity.popupshow = true;
                        myChampIdentity.IsSessionTaken = true;
                        _httpContextAccessor.HttpContext.Session.SetString("popUpShow", "true");
                        _httpContextAccessor.HttpContext.Session.SetString("username", user.EmailId);
                        _httpContextAccessor.HttpContext.Session.SetString("password", login_Model.Password);
                        return 1;
                    }
                    else
                    {
                        var result = await _signInManager.PasswordSignInAsync(login_Model.Name, login_Model.Password, login_Model.RememberMe, false);
                        if (result.Succeeded)
                        {
                            var sessionId = _httpContextAccessor.HttpContext.Session.Id;
                            myChampIdentity.SessionId = sessionId;
                            Console.WriteLine("session id ", sessionId);
                            var username = login_Model.Name;
                            var ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress?.ToString();
                            myChampIdentity.IpAddress = ipAddress;
                            
                            var loginTime = DateTime.UtcNow;
                            _sessionHandlerController.AddSessionInformation(sessionId, username, ipAddress, loginTime,user.Id.ToString()) ;
                            return 2; 
                        }
                        else 
                        {
                            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                            return 3;
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid username or password.");
                }
            }

            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine($"Model error: {error.ErrorMessage}");
            }


            return 4;
        }
    }
}
