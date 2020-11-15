using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DataLayer;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer;
using WebLayer.Viewmodels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebLayer.Controllers
{
    [Consumes("application/json")]
    [Produces("application/json")]
    [Route("api/events")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private IEventService eventService;
        private IMapper mapper;

        public EventsController(IEventService eventService, IMapper mapper)
        {
            this.eventService = eventService;
            this.mapper = mapper;
        }
        // GET: api/events
        [HttpGet]
        public async Task<ActionResult<IList<EventDetails>>> GetAll()
        {
            IList<Event> allEventsFromService = await this.eventService.GetAllAsync();
            IList<EventDetails> allEvents = this.mapper.Map<IList<EventDetails>>(allEventsFromService);

            return Ok(allEvents);
        }

        // GET api/events/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EventDetails>> GetById(int id)
        {
            Event eventFromService = await this.eventService.GetByIdAsync(id);
            if (eventFromService == null)
            {
                return this.NotFound();
            }

            EventDetails eventDetails = this.mapper.Map<EventDetails>(eventFromService);

            return Ok(eventDetails);
        }

        // POST api/events
        [HttpPost]
        public async Task<ActionResult<Event>> CreateEvent([FromBody] EventFormModel newEvent)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest();
            }

            Event eventToCreate = this.mapper.Map<Event>(newEvent);
            await eventService.CreateEventAsync(eventToCreate);

            return CreatedAtAction(nameof(GetById), new { id = eventToCreate.Id }, eventToCreate);
        }

        // PUT api/events/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Event>> UpdateEvent(int id, [FromBody] EventFormModel eventToUpdate)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest();
            }

            Event eventFromService = await this.eventService.GetByIdAsync(id);

            if (eventFromService == null)
            {
                return this.NotFound();
            }

            this.mapper.Map(eventToUpdate, eventFromService);
            await eventService.UpdateEventAsync(eventFromService);

            return Ok(eventFromService);
        }

        // DELETE api/events/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEvent(int id)
        {
            Event eventToDelete = await this.eventService.GetByIdAsync(id);
            if (eventToDelete == null)
            {
                return this.NotFound();
            }

            await this.eventService.DeleteEventAsync(eventToDelete);

            return this.NoContent();
        }
    }
}
