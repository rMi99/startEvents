using Microsoft.EntityFrameworkCore;
using online_event_booking.Business;
using online_event_booking.Data;
using online_event_booking.Data.Entities;

namespace online_event_booking.Repository
{
    public class EventRepository : IEventManagement
    {
        private readonly ApplicationDbContext _context;

        public EventRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Event>> GetAllAsync()
        {
            return await _context.Events
                .Include(e => e.Venue)
                .Include(e => e.Prices)
                .Where(e => e.DeletedAt == null)
                .ToListAsync();
        }

        public List<Event> GetAll()
        {
            var events = _context.Events
                .Include(e => e.Venue)
                .Include(e => e.Prices)
                .Where(e => e.DeletedAt == null)
                .ToList();
            return events;
        }

        public async Task<Event> GetByIdAsync(int id)
        {
            return await _context.Events
                .Include(e => e.Venue)
                .Include(e => e.Prices)
                .FirstOrDefaultAsync(m => m.Id == id && m.DeletedAt == null);
        }

        public Event GetById(int id)
        {
            var eventItem = _context.Events
                .Include(e => e.Venue)
                .Include(e => e.Prices)
                .FirstOrDefault(m => m.Id == id && m.DeletedAt == null);
            return eventItem;
        }

        public async Task CreateEventAsync(Event request)
        {
            try
            {
                request.CreatedAt = DateTime.Now;
                await _context.AddAsync(request);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while creating event: {ex.Message}");
                throw;
            }
        }

        public void CreateEvent(Event request)
        {
            try
            {
                request.CreatedAt = DateTime.Now;
                _context.Add(request);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while creating event: {ex.Message}");
                throw;
            }
        }

        public async Task UpdateEventAsync(Event request)
        {
            try
            {
                request.ModifiedAt = DateTime.Now;
                _context.Update(request);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while updating event: {ex.Message}");
                throw;
            }
        }

        public void UpdateEvent(Event request)
        {
            try
            {
                request.ModifiedAt = DateTime.Now;
                _context.Update(request);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while updating event: {ex.Message}");
                throw;
            }
        }

        public async Task DeleteEventAsync(Event request)
        {
            try
            {
                // Soft delete - set DeletedAt timestamp
                request.DeletedAt = DateTime.Now;
                _context.Update(request);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while deleting event: {ex.Message}");
                throw;
            }
        }

        public void DeleteEvent(Event request)
        {
            try
            {
                // Soft delete - set DeletedAt timestamp
                request.DeletedAt = DateTime.Now;
                _context.Update(request);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while deleting event: {ex.Message}");
                throw;
            }
        }
    }
}