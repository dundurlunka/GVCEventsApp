using DataLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class EventService : IEventService
    {
        private EventsDbContext _context;

        public EventService(EventsDbContext context)
        {
            this._context = context;
        }

        public async Task<IList<Event>> GetAllAsync()
        {
            return await this._context.Events.ToListAsync();
        }

        public async Task<Event> GetByIdAsync(int id)
        {
            return await this._context.Events.FindAsync(id);
        }

        public async Task CreateEventAsync(Event eventToCreate)
        {
            eventToCreate.StartDate = eventToCreate.StartDate.AddHours(2);
            await this._context.Events.AddAsync(eventToCreate);
            await this._context.SaveChangesAsync();
        }

        public async Task UpdateEventAsync(Event eventToUpdate)
        {
            eventToUpdate.StartDate = eventToUpdate.StartDate.AddHours(2);
            this._context.Events.Update(eventToUpdate);
            await this._context.SaveChangesAsync();
        }

        public async Task DeleteEventAsync(Event eventToDelete)
        {
            this._context.Events.Remove(eventToDelete);
            await this._context.SaveChangesAsync();
        }
    }
}
