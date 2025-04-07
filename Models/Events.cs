using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace EventEase.Models
{
    public class Event
    {
        [Key]
        public int EventId { get; set; }

        [Required(ErrorMessage = "Event name is required")]
        [StringLength(100, ErrorMessage = "Event name cannot exceed 100 characters")]
        public string EventName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Event date is required")]
        [DataType(DataType.DateTime)]
        public DateTime EventDate { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Venue is required")]
        [ForeignKey("VenueId")]
        public int VenueId { get; set; }

        // Navigation properties
        public Venue? Venue { get; set; }
        public ICollection<Booking> Bookings { get; set; } = new HashSet<Booking>();

        // Image handling
        public string? ImagePath { get; set; }

        [NotMapped]
        [Display(Name = "Event Image")]
        public IFormFile? ImageFile { get; set; }
    }
}