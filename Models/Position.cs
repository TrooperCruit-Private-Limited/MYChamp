using System.ComponentModel.DataAnnotations;

namespace MYChamp.Models
{
    public class Position
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Range(0, 50)]
        public int AllottedLeaves { get; set; }
        public bool IsActiveP {  get; set; }
    }
}
