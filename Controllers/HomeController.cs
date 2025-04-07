//Controllers/HomeController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventEase.Models;
using EventEase.Models.ViewsModels;
using System.Linq;
using System.Threading.Tasks;
using EventEase.Models.ViewsModels;

namespace EventEase.Controllers
{
    public class HomeController : Controller
    {
        private readonly EventEaseDbContext _context;

        public HomeController(EventEaseDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var model = new DashboardViewModel
            {
                TotalEvents = _context.Events.Count(),
                TotalVenues = _context.Venues.Count(),
                FeaturedEvents = _context.Events
                    .OrderByDescending(e => e.EventDate)
                    .Take(3)
                    .Include(e => e.Venue)
                    .ToList(),
                PopularVenues = _context.Venues
                    .OrderByDescending(v => v.Events.Count)
                    .Take(3)
                    .ToList()
            };
            return View(model);
        }
    }
}
