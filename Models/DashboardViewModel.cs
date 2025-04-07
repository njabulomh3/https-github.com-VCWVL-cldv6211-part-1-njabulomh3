namespace EventEase.Models
{
    public class DashboardViewModel
    {
        public int TotalEvents { get; set; }
        public int TotalVenues { get; set; }
        public List<Event> FeaturedEvents { get; set; }
        public List<Venue> PopularVenues { get; set; }
    }
}