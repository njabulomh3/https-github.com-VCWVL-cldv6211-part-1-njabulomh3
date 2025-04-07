using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventEase.Models
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }

        [Required(ErrorMessage = "Event is required")]
        [Display(Name = "Event")]
        public int EventId { get; set; }

        [Required(ErrorMessage = "Venue is required")]
        [Display(Name = "Venue")]
        public int VenueId { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime BookingDate { get; set; } = DateTime.UtcNow;

        public string UserId { get; set; }  // Remove [Required] - will be set automatically

        // Navigation properties
        [ForeignKey("EventId")]
        public Event? Event { get; set; }

        [ForeignKey("VenueId")]
        public Venue? Venue { get; set; }
    }
}
