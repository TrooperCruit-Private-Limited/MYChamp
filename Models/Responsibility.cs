using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MYChamp.Models
{
    public class Responsibility
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string ResponsibilityName { get; set; } 

        public string? Description { get; set; }

        public ICollection<MenuItem> MenuItems { get; set; }


    }
}
