namespace EventEase.Models.ViewModels
{
    public class EventListingViewModel
    {
        public int EventId { get; set; }
        public string EventName { get; set; }
        public DateTime EventDate { get; set; }
        public string ImagePath { get; set; }

        // Optional venue info
        public string VenueName { get; set; }
        public string VenueLocation { get; set; }
        public bool ShowVenueInfo { get; set; }
    }
}
