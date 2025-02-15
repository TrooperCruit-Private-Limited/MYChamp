using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MYChamp.Models
{
    public class LeaveDetail
    {
        [Key]
        public int Id { get; set; }

        public string LeaveApplicationId { get; set; }

        public DateTime LeaveDate { get; set; }

        public string LeaveType { get; set; } // Full Day, Half Day - Morning, Half Day - Afternoon
    }
}
