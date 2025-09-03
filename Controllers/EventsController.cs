using Microsoft.AspNetCore.Mvc;
using online_event_booking.Data;
using online_event_booking.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using online_event_booking.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;

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


        //GET: Event Create

        public IActionResult Create()
        {
            ViewData["VenueId"] = new SelectList(_context.Venues, "Id", "Name");
            return View();
        }

        [HttpPost]

        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create([Bind("Title,Description,Category,EventDate,Eventtime,VenueId")] Event eventModel)
        {
            if (ModelState.IsValid)
            {

                _context.Add(eventModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["VenueId"] = new SelectList(_context.Venues, "Id", "Name", eventModel.VenueId);
            return View(eventModel);


        }


        [HttpPost]

        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Category,EventDate,Eventtime,VenueId")] Event eventModel)
        {
            if (id != eventModel.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eventModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(eventModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["VenueId"] = new SelectList(_context.Venues, "Id", "Name", eventModel.VenueId);
            return View(eventModel);
        }

        private bool EventExists(int id)
        {
            throw new NotImplementedException();
        }


        //GET : Events/DELETE/5

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Events == null)
            {
                return NotFound();
            }
            var eventModel = await _context.Events
                .Include(e => e.Venue)
                .FirstOrDefaultAsync(m => m.Id == id);


            if (eventModel == null)
            {
                return NotFound();
            }
            return View(eventModel);
        }


        //Post: Event/Delete/5

        [HttpPost, ActionName("Delete")]

        [ValidateAntiForgeryToken]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Events == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Events'  is null.");
            }
            var eventModel = await _context.Events.FindAsync(id);
            if (eventModel != null)
            {
                _context.Events.Remove(eventModel);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        // GET: Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventItem = await _context.Events
                .Include(e => e.Venue)
                .Include(e => e.Prices)
                .FirstOrDefaultAsync(m => m.Id == id.Value);

            if (eventItem == null)

                return NotFound();


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
