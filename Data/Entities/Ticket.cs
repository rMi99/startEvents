namespace online_event_booking.Data.Entities
{
    public class Ticket
    {
        public int Id { get; set; }

        public string CustomerId { get; set; } = default!;
        public ApplicationUser Customer { get; set; } = default!;

        public int EventId { get; set; }
        public Event Event { get; set; } = default!;

        public int EventPriceId { get; set; }
        public EventPrice EventPrice { get; set; } = default!;

        public string TicketNumber { get; set; } = default!;
        public string TicketCode { get; set; } = default!; // Added for ticket code
        public int Quantity { get; set; } = 1; // Added for quantity
        public decimal TotalAmount { get; set; } // Added for total amount
        public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;
        public bool IsPaid { get; set; } = false;
        public string QrCodePath { get; set; } = default!;
    }
}
