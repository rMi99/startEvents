using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using online_event_booking.Data;
using online_event_booking.Data.Entities;

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
            var events = await _context.Events.Include(e => e.Venue).ToListAsync();
            return View(events);
        }

        // GET: Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var eventItem = await _context.Events
                .Include(e => e.Venue)
                .Include(e => e.Prices)
                .FirstOrDefaultAsync(m => m.Id == id.Value);

            if (eventItem == null) return NotFound();

            return View(eventItem);
        }

        // GET: Events/Create
        public IActionResult Create()
        {
            ViewData["VenueId"] = new SelectList(_context.Venues, "Id", "Name");
            return View();
        }

        // POST: Events/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Event eventModel)
        {
            if (ModelState.IsValid)
            {
                eventModel.CreatedAt = DateTime.Now;
                _context.Add(eventModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["VenueId"] = new SelectList(_context.Venues, "Id", "Name", eventModel.VenueId);
            return View(eventModel);
        }

        // GET: Events/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var eventItem = await _context.Events
                .Include(e => e.Venue)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (eventItem == null)
            {
                return NotFound(); // Event not found
            }

            // Populate dropdown for venues
            ViewBag.Venues = new SelectList(_context.Venues, "Id", "Name", eventItem.VenueId);

            return View(eventItem); // Show the edit form
        }

        // POST: Events/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Event eventModel)
        {
            if (id != eventModel.Id)
            {
                return NotFound(); // Ensure correct event is updated
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Track update timestamp
                    eventModel.UpdatedAt = DateTime.Now;

                    // Update the event in the database
                    _context.Events.Update(eventModel);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Event updated successfully!";
                    return RedirectToAction(nameof(Edit), new { id = eventModel.Id }); // Stay on same edit page
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Events.Any(e => e.Id == eventModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            // Reload dropdown if validation fails
            ViewBag.Venues = new SelectList(_context.Venues, "Id", "Name", eventModel.VenueId);
            return View(eventModel);
        }


        // GET: Events/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var eventItem = await _context.Events
                .Include(e => e.Venue)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (eventItem == null)
            {
                return NotFound();
            }

            return View(eventItem); // shows confirmation page
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eventItem = await _context.Events.FindAsync(id);
            if (eventItem != null)
            {
                _context.Events.Remove(eventItem);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index)); // reloads admin/events page
        }
    }
}
