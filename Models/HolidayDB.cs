using System.ComponentModel.DataAnnotations;

namespace MYChamp.Models
{
    public class HolidayDB
    {
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTimeOffset HolidayDate { get; set; }

        [Required]
        [MaxLength(100)]
        public string ? HolidayName { get; set; }

        [Required]
        [MaxLength(100)]
        public string? BranchName { get; set; }

        [Range(1, 25, ErrorMessage = "Holiday limit must be between 1 and 25.")]
        public int Limit { get; set; }

    }
}
