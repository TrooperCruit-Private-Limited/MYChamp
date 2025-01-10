using System.ComponentModel.DataAnnotations;

namespace MYChamp.Models
{
    public class PunchModel
    {
        [Key]
        public int UserID { get; set; }
        public DateTime PunchTime { get; set; }
    }
}