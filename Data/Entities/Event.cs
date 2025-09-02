using online_event_booking.Helper;
using System.Net.Sockets;

namespace online_event_booking.Data.Entities
{
    public class Event: CommonProps
    {
        public int Id { get; set; }
        public int VenueId { get; set; }
        public Venue Venue { get; set; } = default!;
        public string OrganizerId { get; set; } = default!;
        public ApplicationUser Organizer { get; set; } = default!;

        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;
        public DateTime EventDate { get; set; }
        public DateTime EventTime { get; set; }
        public string Category { get; set; } = default!;
        public string? Image { get; set; }
        public bool IsPublished { get; set; }

        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
        public ICollection<EventPrice> Prices { get; set; } = new List<EventPrice>();
    }
}
