using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventEase.Models;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace EventEase.Controllers
{
    [Authorize]
    public class VenuesController : Controller
    {
        private readonly EventEaseDbContext _context;
        private readonly ILogger<VenuesController> _logger;
        private readonly IWebHostEnvironment _hostEnvironment;

        public VenuesController(
            EventEaseDbContext context,
            ILogger<VenuesController> logger,
            IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _logger = logger;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult DebugVenues()
        {
            var venues = _context.Venues.ToList();
            return Json(venues);
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Venues.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venue = await _context.Venues
                .Include(v => v.Events)
                .FirstOrDefaultAsync(m => m.VenueId == id);

            if (venue == null)
            {
                return NotFound();
            }

            return View(venue);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Venue venue)
        {
            if (ModelState.IsValid)
            {
                if (venue.ImageFile != null && venue.ImageFile.Length > 0)
                {
                    var uploadsDir = Path.Combine(_hostEnvironment.WebRootPath, "images/venues");
                    Directory.CreateDirectory(uploadsDir);
                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(venue.ImageFile.FileName);
                    var filePath = Path.Combine(uploadsDir, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await venue.ImageFile.CopyToAsync(stream);
                    }

                    venue.ImageUrl= "/images/venues/" + uniqueFileName;
                }

                _context.Add(venue);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(venue);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venue = await _context.Venues.FindAsync(id);
            if (venue == null)
            {
                return NotFound();
            }
            return View(venue);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("VenueId,VenueName,Location,Capacity,ImageUrl,ImageFile")] Venue venue)
        {
            if (id != venue.VenueId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    if (venue.ImageFile is IFormFile imageFile && imageFile.Length > 0)
                    {
                        if (!string.IsNullOrEmpty(venue.ImageUrl))
                        {
                            var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, venue.ImageUrl.TrimStart('/'));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        var uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "images", "venues");
                        Directory.CreateDirectory(uploadsFolder);
                        var fileName = Path.GetFileName(imageFile.FileName);
                        var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(fileStream);
                        }

                        venue.ImageUrl = $"/images/venues/{uniqueFileName}";
                    }

                    _context.Update(venue);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error editing venue");
                    ModelState.AddModelError("", "An error occurred while editing the venue. Please try again.");
                }
            }
            return View(venue);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venue = await _context.Venues
                .Include(v => v.Events)
                .FirstOrDefaultAsync(m => m.VenueId == id);

            if (venue == null)
            {
                return NotFound();
            }

            return View(venue);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var venue = await _context.Venues
                .Include(v => v.Events)
                .FirstOrDefaultAsync(v => v.VenueId == id);

            if (venue == null)
            {
                return NotFound();
            }

            try
            {
                if (!string.IsNullOrEmpty(venue.ImageUrl))
                {
                    var imagePath = Path.Combine(_hostEnvironment.WebRootPath, venue.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                _context.Venues.Remove(venue);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting venue");
                ModelState.AddModelError("", "An error occurred while deleting the venue. Please try again.");
                return View("Delete", venue);
            }
        }

        private bool VenueExists(int id)
        {
            return _context.Venues.Any(e => e.VenueId == id);
        }
    }
}
