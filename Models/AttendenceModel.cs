using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MYChamp.Models
{
    public class AttendenceModel
    {
        [Required]
        public int UserID { get; set; }
        [Key]
        [Column("Marked Attendence")]
        public int MarkedAttendence { get; set; }

        public DateTime DateTime { get; set; }
        
        
    }
}
