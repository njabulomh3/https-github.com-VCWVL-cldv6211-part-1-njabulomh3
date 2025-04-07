using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventEase.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;


namespace EventEase.Controllers
{
    [Authorize]
    public class BookingsController : Controller
    {
        private readonly EventEaseDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public BookingsController(EventEaseDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Bookings/Create
        public async Task<IActionResult> Create(int eventId)
        {
            var @event = await _context.Events
                .Include(e => e.Venue)
                .FirstOrDefaultAsync(e => e.EventId == eventId);

            if (@event == null)
            {
                return NotFound();
            }

            var booking = new Booking
            {
                EventId = eventId,
                VenueId = @event.VenueId, // Automatically set from event
                UserId = _userManager.GetUserId(User) // Automatically set from logged-in user
            };

            ViewBag.EventName = @event.EventName;
            ViewBag.VenueName = @event.Venue.VenueName;

            return View(booking);
        }

        // POST: Bookings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EventId,VenueId")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                // Set automatic fields
                booking.UserId = _userManager.GetUserId(User);
                booking.BookingDate = DateTime.UtcNow;

                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Repopulate view data if validation fails
            var @event = await _context.Events
                .Include(e => e.Venue)
                .FirstOrDefaultAsync(e => e.EventId == booking.EventId);

            ViewBag.EventName = @event?.EventName;
            ViewBag.VenueName = @event?.Venue?.VenueName;

            return View(booking);
        }
    }
}