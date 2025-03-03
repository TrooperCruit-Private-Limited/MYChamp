using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MYChamp.Models
{
    public class SlotBookingDetails
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("EmployeeList")] // Foreign key to EmployeeList
        public int EmployeeId { get; set; }

        // Navigation property to EmployeeList
        public EmployeeList Employee { get; set; }

        [Required]
        public DateTime SlotDate { get; set; } // 🔹 Date for the slot

        [Required]

        public TimeSpan StartTime { get; set; } // 🔹 Start time of the slot

        [Required]

        public TimeSpan EndTime { get; set; } // 🔹 End time of the slot

        [Required]
        public bool IsBooked { get; set; } = false; // 🔹 Indicates if the slot is booked

        [Required]
        public bool IsCanceled { get; set; } = false; // 🔹 Indicates if the booking was canceled

        [StringLength(100)]
        public string? BookedBy { get; set; } // 🔹 Name of the person who booked the slot

        [Required]
        public string? BookedByEmail { get; set; } // 🔹 Email of the person who booked

        [StringLength(15)]
        public string? BookedByPhone { get; set; } // 🔹 Phone number of the person who booked

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now; // 🔹 Record creation timestamp

        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.Now; // 🔹 Last updated timestamp
        [Required]
        public bool IsConfirmed { get; set; } = false; // 🔹 Indicates if booking is confirmed by admin

        //  public bool IsAutoBooked { get; set; } = false; // 🔹 Indicates if booking was made via AutoBooking

        // public string? Notes { get; set; } // 🔹 Optional notes about the booking

        // 🔹 Method to handle cancellations
        
    }
}
