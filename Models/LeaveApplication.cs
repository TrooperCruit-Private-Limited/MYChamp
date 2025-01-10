using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MYChamp.Models
{
    public class LeaveApplication
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserID { get; set; }

        [Required]
        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }
        [ValidateNever]
        public Employee Employee { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime FromDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ToDate { get; set; }

        [Required]
        public string Reason { get; set; }

        [Display(Name = "Date Of Request")]
        public DateTime DateRequested { get; set; }

        public double NumberOfDays { get; set; }
        [ValidateNever]
        public string Status { get; set; }

        [ForeignKey("Manager")]
        public int ManagerId { get; set; }
       
        [ValidateNever]
        public Employee Manager { get; set; }

        public bool IsHalfDay { get; set; } // Indicates half-day leave

        [ValidateNever]
        public List<LeaveDetail> LeaveDetails { get; set; } // LeaveDetails for each leave application

    }
}
