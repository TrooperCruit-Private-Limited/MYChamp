using System.ComponentModel.DataAnnotations;


namespace MYChamp.Models
{
    public class Holiday
    {
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [FutureDate(ErrorMessage = "Date must be in the future.")]
        public DateTimeOffset HolidayDate { get; set; }

        [Required]
        [MaxLength(100)]
        public string HolidayName { get; set; }
    }
}
