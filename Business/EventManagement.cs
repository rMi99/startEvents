using online_event_booking.Data.Entities;
using online_event_booking.Repository;

namespace online_event_booking.Business
{
    public class EventManagement : IEventManagement
    {
        private readonly IEventRepository _eventRepository;

        public EventManagement(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public List<Event> GetAll()
        {
            return _eventRepository.GetAll();
        }

        public void CreateEvent(Event request)
        {
            _eventRepository.CreateEvent(request);
        }

        public Event GetById(int id)
        {
            return _eventRepository.GetById(id);
        }

        public void UpdateEvent(Event request)
        {
            request.ModifiedAt = DateTime.Now; // optional if you want to track updates
            _eventRepository.UpdateEvent(request);
        }

        public void DeleteEvent(Event request)
        {
            request.DeletedAt = DateTime.Now; // optional soft delete
            _eventRepository.DeleteEvent(request);
        }
    }
}
