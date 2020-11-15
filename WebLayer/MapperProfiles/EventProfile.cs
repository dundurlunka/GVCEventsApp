using AutoMapper;
using DataLayer;
using WebLayer.Viewmodels;

namespace WebLayer.MapperProfiles
{
    public class EventProfile : Profile
    {
        public EventProfile()
        {
            CreateMap<EventFormModel, Event>();
            CreateMap<Event, EventDetails>();
            CreateMap<EventDetails, Event>();
        }
    }
}
