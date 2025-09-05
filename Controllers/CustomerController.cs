using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using online_event_booking.Data;
using online_event_booking.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace online_event_booking.Controllers
{
    [Authorize(Roles = "Customer")]
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CustomerController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Customer Dashboard";
            ViewData["ActivePage"] = "Dashboard";
            
            var userId = _userManager.GetUserId(User);
            if (userId == null) return RedirectToAction("Login", "Account");
            
            var user = await _userManager.FindByIdAsync(userId);

            // Get customer's booking statistics
            var myTickets = await _context.Tickets
                .Where(t => t.CustomerId == userId)
                .Include(t => t.Event)
                .ThenInclude(e => e.Venue)
                .ToListAsync();

            var totalSpent = myTickets.Where(t => t.IsPaid).Sum(t => t.TotalAmount);

            var loyaltyPoints = await _context.LoyaltyPoints
                .Where(lp => lp.CustomerId == userId)
                .SumAsync(lp => lp.Points);

            var dashboardData = new
            {
                TotalTickets = myTickets.Count,
                UpcomingEvents = myTickets.Where(t => t.Event.EventDate > DateTime.Now).Count(),
                TotalSpent = totalSpent,
                LoyaltyPoints = loyaltyPoints,
                RecentTickets = myTickets.OrderByDescending(t => t.PurchaseDate).Take(5).ToList(),
                UpcomingEventsList = myTickets.Where(t => t.Event.EventDate > DateTime.Now)
                                            .OrderBy(t => t.Event.EventDate).Take(3).ToList()
            };

            return View(dashboardData);
        }

        public async Task<IActionResult> MyTickets()
        {
            ViewData["Title"] = "My Tickets";
            ViewData["ActivePage"] = "MyTickets";
            
            var userId = _userManager.GetUserId(User);
            var tickets = await _context.Tickets
                .Where(t => t.CustomerId == userId)
                .Include(t => t.Event)
                .ThenInclude(e => e.Venue)
                .OrderByDescending(t => t.PurchaseDate)
                .ToListAsync();
                
            return View(tickets);
        }

        public async Task<IActionResult> BrowseEvents()
        {
            ViewData["Title"] = "Browse Events";
            
            var events = await _context.Events
                .Where(e => e.EventDate > DateTime.Now)
                .Include(e => e.Venue)
                .Include(e => e.Prices)
                .OrderBy(e => e.EventDate)
                .ToListAsync();
                
            return View(events);
        }

        public async Task<IActionResult> EventDetails(int id)
        {
            ViewData["Title"] = "Event Details";
            
            var eventEntity = await _context.Events
                .Include(e => e.Venue)
                .Include(e => e.Prices)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (eventEntity == null)
            {
                return NotFound();
            }

            return View(eventEntity);
        }

        [HttpPost]
        public async Task<IActionResult> BookTicket(int eventId, int priceId, int quantity)
        {
            var userId = _userManager.GetUserId(User);
            var eventEntity = await _context.Events.FindAsync(eventId);
            var price = await _context.EventPrices.FindAsync(priceId);

            if (eventEntity == null || price == null || quantity <= 0 || userId == null)
            {
                TempData["Error"] = "Invalid booking request.";
                return RedirectToAction(nameof(BrowseEvents));
            }

            // Check stock availability
            if (price.Stock < quantity)
            {
                TempData["Error"] = "Not enough tickets available.";
                return RedirectToAction(nameof(EventDetails), new { id = eventId });
            }

            // Create ticket
            var ticket = new Ticket
            {
                EventId = eventId,
                CustomerId = userId,
                EventPriceId = priceId,
                Quantity = quantity,
                PurchaseDate = DateTime.Now,
                TotalAmount = price.Price * quantity,
                TicketCode = Guid.NewGuid().ToString("N")[..8].ToUpper(),
                TicketNumber = $"TKT-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..6].ToUpper()}",
                IsPaid = true
            };

            _context.Tickets.Add(ticket);

            // Update stock
            price.Stock -= quantity;

            // Create payment record
            var payment = new Payment
            {
                CustomerId = userId,
                TicketId = ticket.Id,
                Amount = ticket.TotalAmount,
                PaymentDate = DateTime.Now,
                Status = "Completed",
                PaymentMethod = "Card" // Default for demo
            };

            _context.Payments.Add(payment);

            // Add loyalty points (1 point per dollar spent)
            var loyaltyPoints = new LoyaltyPoint
            {
                CustomerId = userId,
                Points = (int)ticket.TotalAmount,
                EarnedDate = DateTime.Now,
                Description = $"Booking for {eventEntity.Title}"
            };

            _context.LoyaltyPoints.Add(loyaltyPoints);

            await _context.SaveChangesAsync();

            TempData["Success"] = "Ticket booked successfully!";
            return RedirectToAction(nameof(MyTickets));
        }

        public async Task<IActionResult> Profile()
        {
            ViewData["Title"] = "My Profile";
            ViewData["ActivePage"] = "Profile";
            
            var userId = _userManager.GetUserId(User);
            if (userId == null) return RedirectToAction("Login", "Account");
            
            var user = await _userManager.FindByIdAsync(userId);
            
            return View(user);
        }

        public async Task<IActionResult> LoyaltyPoints()
        {
            ViewData["Title"] = "Loyalty Points";
            ViewData["ActivePage"] = "LoyaltyPoints";
            
            var userId = _userManager.GetUserId(User);
            if (userId == null) return RedirectToAction("Login", "Account");
            
            var loyaltyPoints = await _context.LoyaltyPoints
                .Where(lp => lp.CustomerId == userId)
                .OrderByDescending(lp => lp.EarnedDate)
                .ToListAsync();
                
            return View(loyaltyPoints);
        }
    }
}
