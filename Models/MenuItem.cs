using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MYChamp.Models
{
    public class MenuItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Route { get; set; }

        // Foreign key for Responsibility
        public int ResponsibilityId { get; set; }

        [ForeignKey("ResponsibilityId")]
        public Responsibility Responsibility { get; set; }
    }

}
