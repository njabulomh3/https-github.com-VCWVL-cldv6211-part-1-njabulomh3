// ViewModels/HomeViewModel.cs
using EventEase.Models;
using NuGet.Protocol.Plugins;
using System.Collections.Generic;
using EventEase.Models.ViewsModels; 

namespace EventEase.Models.ViewsModels
{
    public class HomeViewModel
    {
        public IEnumerable<Event> UpcomingEvents { get; set; }
        public IEnumerable<Venue> PopularVenues { get; set; }
        public IEnumerable<Event> FeaturedEvents { get; set; }
        // Add other properties you need for your home page
        public int TotalEvents { get; set; }
        public int TotalVenues { get; set; }

    }
}
