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
            
            var users = await _userManager.Users.ToListAsync();
            var userRoles = new Dictionary<string, IList<string>>();
            
            int adminCount = 0, organizerCount = 0, customerCount = 0;
            
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userRoles[user.Id] = roles;
                
                // Count users by role
                if (roles.Contains("Admin"))
                    adminCount++;
                else if (roles.Contains("Organizer"))
                    organizerCount++;
                else
                    customerCount++;
            }
            
            ViewBag.UserRoles = userRoles;
            ViewBag.TotalUsers = users.Count;
            ViewBag.AdminUsers = adminCount;
            ViewBag.OrganizerUsers = organizerCount;
            ViewBag.CustomerUsers = customerCount;
            
            return View(users);
        }

        public async Task<IActionResult> Events()
        {
            ViewData["Title"] = "Event Management";
            
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
            return View();
        }

        public IActionResult Settings()
        {
            ViewData["Title"] = "System Settings";
            return View();
        }
    }
}
