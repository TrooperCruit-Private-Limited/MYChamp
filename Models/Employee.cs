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

        public double RemainingLeaves { get; set; }
        public int? ReportingManagerId { get; set; }

        public int Salary { get; set; }


        [Column("reportingmanagername")]
        public string? ReportingManagerName { get; set; }

        public bool IsActive { get; set; }

        // New property to hold selected Responsibility IDs
        public List<int> SelectedResponsibilityIds { get; set; } = new List<int>();

        // public ICollection<Responsibility> Responsibilities { get; set; } = new List<Responsibility>();
    }
}
