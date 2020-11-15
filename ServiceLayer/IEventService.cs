using DataLayer;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public interface IEventService
    {
        Task CreateEventAsync(Event eventToCreate);
        Task DeleteEventAsync(Event eventToDelete);
        Task<IList<Event>> GetAllAsync();
        Task<Event> GetByIdAsync(int id);
        Task UpdateEventAsync(Event eventToUpdate);
    }
}