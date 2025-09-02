using online_event_booking.Helper;

namespace online_event_booking.Data.Entities
{
    public class Venue: CommonProps
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Location { get; set; } = default!;
        public int Capacity { get; set; }

        public ICollection<Event> Events { get; set; } = new List<Event>();
    }
}
