using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MYChamp.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        public int EmployeeId { get; set; }

        [Required]
        public string Name { get; set; }
        public string Email { get; set; }
        [Required]
        public int PositionId { get; set; }
        [ValidateNever]
        public Position Position { get; set; }
        public int RemainingLeaves { get; set; }
        public int? ReportingManagerId { get; set; }
        [Column("reportingmanagername")]
        public string? ReportingManagerName { get; set; }

        public List<int>? ResponsibilityId { get; set; } = new List<int>();
        public List<Responsibility>? Responsibilities { get; set; } 

        public bool IsActive { get; set; }
    }
}
