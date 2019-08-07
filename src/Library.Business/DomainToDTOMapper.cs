using AutoMapper;
using Library.Business.DTO;
using Library.Business.EventHandling;

namespace Library.Business
{
    public class DomainToDTOMapper : Profile
    {
        public DomainToDTOMapper()
        {
            CreateMap<Domain.Book, Book>()
                .ForMember(b => b.Id, o => o.MapFrom(b => b.Id))
                .ForMember(b => b.Name, o => o.MapFrom(b => b.Name))
                .ForMember(b => b.Date, o => o.MapFrom(b => b.Date))
                .ForMember(b => b.Rate, o => o.MapFrom(b => b.Rate))
                .ForMember(b => b.Summary, o => o.MapFrom(b => b.Summary))
                .ForMember(b => b.Picture, o => o.MapFrom(b => b.Picture));

            CreateMap<Domain.Author, Author>()
                .ForMember(b => b.Id, o => o.MapFrom(b => b.Id))
                .ForMember(b => b.FirstName, o => o.MapFrom(b => b.Name))
                .ForMember(b => b.LastName, o => o.MapFrom(b => b.SurName))
                .ForMember(b => b.DateOfBirth, o => o.MapFrom(b => b.LifePeriod.DateOfBirth))
                .ForMember(b => b.DateOfDeath, o => o.MapFrom(b => b.LifePeriod.DateOfDeath));

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