using online_event_booking.Data.Entities;

namespace online_event_booking.Repository
{
    public interface IEventRepository
    {
        void CreateEvent(Event request);
        void DeleteEvent(Event request);
        List<Event> GetAll();
        Event GetById(int id);
        void UpdateEvent(object eventItem);
    }
}
