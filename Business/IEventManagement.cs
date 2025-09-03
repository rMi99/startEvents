using online_event_booking.Data.Entities;

namespace online_event_booking.Business
{
    public interface IEventManagement
    {
        List<Event> GetAll();
        void CreateEvent(Event request);
        Event GetById(int id);
        void UpdateEvent(Event request);
        void DeleteEvent(Event request);
    }
}