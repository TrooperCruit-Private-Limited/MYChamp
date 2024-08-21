using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MYChamp.Models
{
    public class MyChampLogin:IdentityUser
    {
        [Required]
     public string? name { get; set; }
        [Required]
        public string? Address { get; set; }
    }
}
