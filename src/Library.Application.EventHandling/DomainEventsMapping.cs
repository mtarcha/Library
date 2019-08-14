using AutoMapper;
using Library.Application.EventHandling.Events;

namespace Library.Application.EventHandling
{
    public class DomainEventsMapping : Profile
    {
        public DomainEventsMapping()
        {

            CreateMap<Domain.Events.RecommendedBookAddedEvent, RecommendedBookAddedEvent>()
                .ForMember(b => b.RaiseTime, o => o.MapFrom(b => b.RaiseTime))
                .ForMember(b => b.BookId, o => o.MapFrom(b => b.BookId))
                .ForMember(b => b.UserId, o => o.MapFrom(b => b.UserId));

            CreateMap<Domain.Events.BookRateChangedEvent, BookRateChangedEvent>()
                .ForMember(b => b.RaiseTime, o => o.MapFrom(b => b.RaiseTime))
                .ForMember(b => b.BookId, o => o.MapFrom(b => b.BookId))
                .ForMember(b => b.Rate, o => o.MapFrom(b => b.Rate));
        }
    }
}