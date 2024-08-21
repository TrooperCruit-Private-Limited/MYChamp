using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MYChamp.AuthModel;
using MYChamp.DbContexts;
using System;
using System.Linq;
using System.Xml;

namespace MYChamp.Controller
{
    public class SessionHandlerController : ControllerBase
    {
        private readonly MYChampDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public SessionHandlerController(MYChampDbContext db, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            Console.WriteLine("checking whether it will print in console");
        }

        public bool SessionExists(string userId)
        {
            var result = _db.sessionlog.FirstOrDefault(s =>s.UserName == userId && s.IsActive);
            if (result != null)
            {
                  
                return true;
            }
            else
            {
                return false;
            }
        }

        public void AddSessionInformation(string sessionId, string username, string ip, DateTime loggedTime,string userid)
        {  
            
            var session = new Session_model
            {
                SessionId = sessionId,
               

                forcefully_logout_by = string.Empty,
                UserName = username,
                IPAddress = ip,
                LoginTime = loggedTime,
                IsActive = true,
                status = 1,
                logoutType = string.Empty
            };
          //  _httpContextAccessor.HttpContext.Session.SetString("uniqeid", session.uniqueId);
            
            
            _db.sessionlog.Add(session);
            _db.SaveChanges();
        }

        public void UpdateSessionInformation( string userName)
        {
            var session = _db.sessionlog.FirstOrDefault(x => x.UserName == userName && x.IsActive);
            Console.Write("session info " + session);
            if (session != null)
            {
                session.IsActive = false;
                session.status = 0;
                session.logoutType = "normal";
                session.LogoutTime= DateTime.UtcNow;

                _db.sessionlog.Update(session);
                _db.SaveChanges();
            }
        }

        public bool check(string uniqueId)
        {
            var obj = _db.sessionlog.FirstOrDefault(x => x.UserName == uniqueId && x.forcefully_logout);
            if (obj != null)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public void UpdateForceLogout(string username)
        {
            var session = _db.sessionlog.FirstOrDefault(x => x.UserName  == username && x.IsActive);
            if (session != null)
            {
                session.forcefully_logout = true;
                session.forcefully_logout_by = username;
                session.IsActive = false;
                session.status = 0;
                session.logoutType = "abnormal";
                session.LogoutTime = DateTime.UtcNow;

                _db.sessionlog.Update(session);
                _db.SaveChanges();
            }
        }

        public bool checkForcefulLogout()
        {
            string uniqueId= string.Empty;
            try
            {
                 uniqueId = _httpContextAccessor.HttpContext.Session.GetString("uniqeid");
            }
            catch
            {
                Console.WriteLine("error");
            }
            
          
            var obj= _db.sessionlog.FirstOrDefault(u=>u.UserName==uniqueId&& u.forcefully_logout);
            if(obj != null)
            {
                return true;
            }
            else
            {
                return false;
            }

        }


        
    }
}
