using Microsoft.AspNetCore.Mvc;
using online_event_booking.Data;
using online_event_booking.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace online_event_booking.Controllers
{
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Events
        public async Task<IActionResult> Index()
        {
            var events = await _context.Events
                .Include(e => e.Venue)
                .Include(e => e.Prices)
                .Where(e => e.EventDate >= DateTime.Now)
                .OrderBy(e => e.EventDate)
                .ToListAsync();

            return View(events);
        }

        // GET: Events/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventItem = await _context.Events
                .Include(e => e.Venue)
                .Include(e => e.Prices)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (eventItem == null)
            {
                return NotFound();
            }

            return View(eventItem);
        }

        // GET: Events/Search
        public async Task<IActionResult> Search(string searchTerm, string category, DateTime? date, string location)
        {
            var query = _context.Events
                .Include(e => e.Venue)
                .Include(e => e.Prices)
                .Where(e => e.EventDate >= DateTime.Now)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(e => e.Title.Contains(searchTerm) || e.Description.Contains(searchTerm));
            }

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(e => e.Category == category);
            }

            if (date.HasValue)
            {
                query = query.Where(e => e.EventDate.Date == date.Value.Date);
            }

            if (!string.IsNullOrEmpty(location))
            {
                query = query.Where(e => e.Venue.Location.Contains(location) || e.Venue.Name.Contains(location));
            }

            var events = await query.OrderBy(e => e.EventDate).ToListAsync();
            ViewBag.SearchTerm = searchTerm;
            ViewBag.Category = category;
            ViewBag.Date = date;
            ViewBag.Location = location;

            return View("Index", events);
        }
    }
}
