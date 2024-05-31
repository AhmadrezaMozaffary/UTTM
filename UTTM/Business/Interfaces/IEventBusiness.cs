using UTTM.Models;
using UTTM.Models.ViewModels;

namespace UTTM.Business.Interfaces
{
    public interface IEventBusiness
    {
        SocietyBusiness SocietyBiz { get; set; }

        Task<int> CreateEvent(EventViewModel e);
        Task<List<Event>> GetAllEvents();
        Task<Event?> GetEventById(int id);
        Task<int> RemoveEvent(int id);
    }
}