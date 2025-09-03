using online_event_booking.Helper;
using System.ComponentModel.DataAnnotations;

namespace online_event_booking.Data.Entities
{
    public class Event : CommonProps
    {
        public int Id { get; set; }

        [Required]
        public int VenueId { get; set; }
        public Venue Venue { get; set; } = default!;

        [Required]
        public string OrganizerId { get; set; } = default!;
        public ApplicationUser Organizer { get; set; } = default!;

        [Required, StringLength(200)]
        public string Title { get; set; } = default!;

        [Required]
        public string Description { get; set; } = default!;

        [Required]
        public DateTime EventDate { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public DateTime EventTime { get; set; }   // we’ll treat this as "time only"

        [Required]
        public string Category { get; set; } = default!;

        public string? Image { get; set; }

        public bool IsPublished { get; set; }

        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
        public ICollection<EventPrice> Prices { get; set; } = new List<EventPrice>();
    }
}
