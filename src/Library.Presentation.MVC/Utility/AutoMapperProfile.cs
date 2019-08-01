using AutoMapper;
using Library.Domain;
using Library.Presentation.MVC.ViewModels;

namespace Library.Presentation.MVC.Utility
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Book, BookViewModel>()
                .ForMember(b => b.Id, o => o.MapFrom(b => b.Id))
                .ForMember(b => b.Date, o => o.MapFrom(b => b.Date))
                .ForMember(b => b.Name, o => o.MapFrom(b => b.Name))
                .ForMember(b => b.Summary, o => o.MapFrom(b => b.Summary))
                .ForMember(b => b.Picture, o => o.MapFrom(b => b.Picture))
                .ForMember(b => b.Rate, o => o.MapFrom(b => b.Rate));

            CreateMap<Book, EditBookViewModel>()
               .ForMember(b => b.Id, o => o.MapFrom(b => b.Id))
               .ForMember(b => b.Date, o => o.MapFrom(b => b.Date))
               .ForMember(b => b.Name, o => o.MapFrom(b => b.Name))
               .ForMember(b => b.Summary, o => o.MapFrom(b => b.Summary))
               .ForMember(b => b.Picture, o => o.MapFrom(b => b.Picture))
               .ForMember(b => b.Avatar, o => o.Ignore())
               .ForMember(b => b.Authors, o => o.MapFrom(b => b.Authors));

            CreateMap<Author, AuthorViewModel>()
              .ForMember(b => b.Id, o => o.MapFrom(b => b.Id))
              .ForMember(b => b.FirstName, o => o.MapFrom(b => b.Name))
              .ForMember(b => b.LastName, o => o.MapFrom(b => b.SurName))
              .ForMember(b => b.DateOfBirth, o => o.MapFrom(b => b.LifePeriod.DateOfBirth))
              .ForMember(b => b.DateOfDeath, o => o.MapFrom(b => b.LifePeriod.DateOfDeath));
        }
    }
}