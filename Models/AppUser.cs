using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MYChamp.Models
{
    public class AppUser:IdentityUser
    {
        [Required]
        public string? name {  get; set; }
        [Required]
        public string? address { get; set; }
    }
}
