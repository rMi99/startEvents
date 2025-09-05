using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using online_event_booking.Data;
using online_event_booking.Data.Entities;
using online_event_booking.Models;
using Microsoft.EntityFrameworkCore;

namespace online_event_booking.Controllers
{
    [Authorize(Roles = "Organizer")]
    public class OrganizerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrganizerController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Organizer Dashboard";
            ViewData["ActivePage"] = "Dashboard";
            
            var userId = _userManager.GetUserId(User);
            if (userId == null) return RedirectToAction("Login", "Account");
            
            var user = await _userManager.FindByIdAsync(userId);

            // Get organizer's events statistics
            var myEvents = await _context.Events
                .Where(e => e.OrganizerId == userId)
                .Include(e => e.Venue)
                .Include(e => e.Prices)
                .ToListAsync();

            var totalTicketsSold = await _context.Tickets
                .Where(t => myEvents.Select(e => e.Id).Contains(t.EventId))
                .SumAsync(t => t.Quantity);

            var totalRevenue = await _context.Tickets
                .Where(t => t.IsPaid && myEvents.Select(e => e.Id).Contains(t.EventId))
                .SumAsync(t => t.TotalAmount);

            var dashboardData = new
            {
                TotalEvents = myEvents.Count,
                PublishedEvents = myEvents.Count(e => e.IsPublished),
                TotalTicketsSold = totalTicketsSold,
                TotalRevenue = totalRevenue,
                MyEvents = myEvents.OrderByDescending(e => e.CreatedAt).Take(5).ToList()
            };

            return View(dashboardData);
        }

        public async Task<IActionResult> MyEvents()
        {
            ViewData["Title"] = "My Events";
            ViewData["ActivePage"] = "MyEvents";
            
            var userId = _userManager.GetUserId(User);
            if (userId == null) return RedirectToAction("Login", "Account");
            
            var events = await _context.Events
                .Where(e => e.OrganizerId == userId)
                .Include(e => e.Venue)
                .Include(e => e.Prices)
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync();
                
            return View(events);
        }

        public IActionResult CreateEvent()
        {
            ViewData["Title"] = "Create New Event";
            ViewData["ActivePage"] = "CreateEvent";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateEvent(Event eventModel)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null) return RedirectToAction("Login", "Account");
            
            if (ModelState.IsValid)
            {
                eventModel.OrganizerId = userId;
                eventModel.CreatedAt = DateTime.Now;
                
                _context.Events.Add(eventModel);
                await _context.SaveChangesAsync();
                
                TempData["Success"] = "Event created successfully!";
                return RedirectToAction(nameof(MyEvents));
            }
            
            return View(eventModel);
        }

        public async Task<IActionResult> EventTickets(int? id)
        {
            ViewData["Title"] = "Event Tickets";
            ViewData["ActivePage"] = "EventTickets";
            
            var userId = _userManager.GetUserId(User);
            
            if (id.HasValue)
            {
                var eventEntity = await _context.Events
                    .Where(e => e.Id == id && e.OrganizerId == userId)
                    .Include(e => e.Venue)
                    .FirstOrDefaultAsync();

                if (eventEntity == null)
                {
                    return NotFound();
                }

                var tickets = await _context.Tickets
                    .Where(t => t.EventId == id)
                    .Include(t => t.Customer)
                    .ToListAsync();

                ViewBag.Event = eventEntity;
                return View(tickets);
            }
            else
            {
                // Show all tickets for organizer's events
                var events = await _context.Events
                    .Where(e => e.OrganizerId == userId)
                    .ToListAsync();
                
                var eventIds = events.Select(e => e.Id).ToList();
                
                var allTickets = await _context.Tickets
                    .Where(t => eventIds.Contains(t.EventId))
                    .Include(t => t.Customer)
                    .Include(t => t.Event)
                    .ToListAsync();

                return View(allTickets);
            }
        }

        public async Task<IActionResult> Analytics()
        {
            ViewData["Title"] = "Event Analytics";
            ViewData["ActivePage"] = "Analytics";
            
            var userId = _userManager.GetUserId(User);
            var events = await _context.Events
                .Where(e => e.OrganizerId == userId)
                .Include(e => e.Venue)
                .ToListAsync();

            return View(events);
        }
    }
}
