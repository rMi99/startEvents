using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using online_event_booking.Data;
using online_event_booking.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace online_event_booking.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Admin Dashboard";
            ViewData["ActivePage"] = "Dashboard";
            
            // Get dashboard statistics
            var totalUsers = await _userManager.Users.CountAsync();
            var totalEvents = await _context.Events.CountAsync();
            var totalTickets = await _context.Tickets.SumAsync(t => t.Quantity);
            var totalRevenue = await _context.Tickets
                .Where(t => t.IsPaid)
                .SumAsync(t => t.TotalAmount);

            var dashboardData = new
            {
                TotalUsers = totalUsers,
                TotalEvents = totalEvents,
                TotalTickets = totalTickets,
                TotalRevenue = totalRevenue,
                RecentUsers = await _userManager.Users
                    .OrderByDescending(u => u.CreatedAt)
                    .Take(5)
                    .ToListAsync(),
                RecentEvents = await _context.Events
                    .Include(e => e.Venue)
                    .OrderByDescending(e => e.CreatedAt)
                    .Take(5)
                    .ToListAsync()
            };

            return View(dashboardData);
        }

        public async Task<IActionResult> Users()
        {
            ViewData["Title"] = "User Management";
            ViewData["ActivePage"] = "Users";
            
            var users = await _userManager.Users.ToListAsync();
            var userRoles = new Dictionary<string, IList<string>>();
            
            foreach (var user in users)
            {
                userRoles[user.Id] = await _userManager.GetRolesAsync(user);
            }
            
            ViewBag.UserRoles = userRoles;
            return View(users);
        }

        public async Task<IActionResult> Events()
        {
            ViewData["Title"] = "Event Management";
            ViewData["ActivePage"] = "Events";
            
            var events = await _context.Events
                .Include(e => e.Venue)
                .Include(e => e.Prices)
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync();
                
            return View(events);
        }

        public IActionResult Reports()
        {
            ViewData["Title"] = "Reports & Analytics";
            ViewData["ActivePage"] = "Reports";
            return View();
        }

        public IActionResult Settings()
        {
            ViewData["Title"] = "System Settings";
            ViewData["ActivePage"] = "Settings";
            return View();
        }
    }
}
