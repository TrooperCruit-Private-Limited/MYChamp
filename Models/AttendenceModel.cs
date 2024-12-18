using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MYChamp.Models
{
   public class AttendenceModel
   {
        public int Id { get; set; }
        public string UserID { get; set; }
        public DateTime DateTime { get; set; }
        public int MarkedAttendence { get; set; } // 1 = Present, 0 = Not Mentioned
        public int LeaveType { get; set; } // 0 = None, 1 = Permission Leave, 2 = Not Mentioned
   }
}