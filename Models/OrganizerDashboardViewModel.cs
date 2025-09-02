using online_event_booking.Data.Entities;

namespace online_event_booking.Models
{
    public class OrganizerDashboardViewModel
    {
        public int TotalEvents { get; set; }
        public int ActiveEvents { get; set; }
        public int TotalTicketsSold { get; set; }
        public decimal TotalRevenue { get; set; }
        public List<Event> RecentEvents { get; set; } = new List<Event>();
        public List<Event> UpcomingEvents { get; set; } = new List<Event>();
    }
}
