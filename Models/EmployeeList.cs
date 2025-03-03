using System;
using System.ComponentModel.DataAnnotations;

public class EmployeeList
{
    [Key]
    public int Id { get; set; }  // Primary Key

    [Required]
    public string Name { get; set; }  // Full Name

    [Required]
    public string JobTitle { get; set; }  // Job Title / Department

    [Required, EmailAddress]
    public string Email { get; set; }

    [Required, Phone]
    public string Phone { get; set; }

    public DateTime SlotDate { get; set; }  // Date of the Slot
    [Required]
    public TimeSpan SlotStart { get; set; }  // Start Time of the Slot

    [Required]
    public TimeSpan SlotEnd { get; set; }  // End Time of the Slot
    [Required]

    public bool Book { get; set; }  // Indicates if the slot is bookable

    public bool IsBooked { get; set; } = false;  // Status of booking

    public bool IsCanceled { get; set; } = false;  // Cancellation status
    [Required]

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    [Required]

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
