using Microsoft.AspNetCore.Identity; // Import this namespace
using MYChamp.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MYChamp.Models
{
    public class SessionModel
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("sessionid")]
        public string SessionId { get; set; }
        [Column("username")]
        public string UserName { get; set; }
        [Column("ipaddress")]
        public string IPAddress { get; set; }
        [Column("logintime")]
        public DateTime LoginTime { get; set; }
        [Column("isactive")]
        public bool IsActive { get; set; }

        public int status { get; set; }
          
        public bool forcefully_logout { get; set; }

        public string forcefully_logout_by { get; set; }
        [Column("logouttype")]
        public string logoutType { get; set; }
        [Column("logouttime")]
        public DateTime LogoutTime {  get; set; }
    }
}
 