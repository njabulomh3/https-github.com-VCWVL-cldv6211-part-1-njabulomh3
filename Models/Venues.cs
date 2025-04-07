using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace EventEase.Models
{
    public class Venue
    {
        [Key]
        public int VenueId { get; set; }

        [Required(ErrorMessage = "Venue name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        [Display(Name = "Venue Name")]
        public string VenueName { get; set; }

        [Required(ErrorMessage = "Location is required")]
        [StringLength(200, ErrorMessage = "Location cannot exceed 200 characters")]
        public string Location { get; set; }

        [Required(ErrorMessage = "Capacity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Capacity must be at least 1")]
        public int Capacity { get; set; }

     

        // Navigation properties
        public ICollection<Event?> Events { get; set; } = new HashSet<Event>();
        public ICollection<Booking?> Bookings { get; set; } = new HashSet<Booking>();

        // Not mapped to database
        [NotMapped]
        [Display(Name = "Venue Image")]
        public IFormFile? ImageFile { get; set; }

        public string? ImageUrl { get; set; }
    }
}
