using System.IO;
using AutoMapper;
using Library.Domain;

namespace Library.Presentation.ViewModels
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
                .ForMember(b => b.Picture, o => o.MapFrom(b => b.Picture));
            //.ForMember(b => b.Rate, o => o.MapFrom(b => b.));

            CreateMap<Book, EditBookViewModel>()
               .ForMember(b => b.Id, o => o.MapFrom(b => b.Id))
               .ForMember(b => b.Date, o => o.MapFrom(b => b.Date))
               .ForMember(b => b.Name, o => o.MapFrom(b => b.Name))
               .ForMember(b => b.Summary, o => o.MapFrom(b => b.Summary))
               .ForMember(b => b.Picture, o => o.MapFrom(b => b.Picture))
               .ForMember(b => b.Avatar, o => o.Ignore());

            CreateMap<CreateBookViewModel, Book>()
               .ForMember(b => b.Date, o => o.MapFrom(b => b.Date))
               .ForMember(b => b.Name, o => o.MapFrom(b => b.Name))
               .ForMember(b => b.Summary, o => o.MapFrom(b => b.Summary))
               .ForMember(b => b.Picture, o => o.MapFrom((b, c, d) =>
                {
                    byte[] imageData = null;

                    using (var binaryReader = new BinaryReader(b.Avatar.OpenReadStream()))
                    {
                        imageData = binaryReader.ReadBytes((int)b.Avatar.Length);
                    }

                    return imageData;
                }));

            CreateMap<EditBookViewModel, Book>()
              .ForMember(b => b.Id, o => o.MapFrom(b => b.Id))
              .ForMember(b => b.Date, o => o.MapFrom(b => b.Date))
              .ForMember(b => b.Name, o => o.MapFrom(b => b.Name))
              .ForMember(b => b.Summary, o => o.MapFrom(b => b.Summary))
              .ForMember(b => b.Picture, o => o.MapFrom((b, c, d) =>
              {
                  if (b.Avatar?.Length > 0)
                  {
                      byte[] imageData = null;

                      using (var binaryReader = new BinaryReader(b.Avatar.OpenReadStream()))
                      {
                          imageData = binaryReader.ReadBytes((int)b.Avatar.Length);
                      }

                      return imageData;
                  }

                  if (b.Picture?.Length > 0)
                  {
                      return b.Picture;
                  }

                  return null;
              }));

            CreateMap<Author, AuthorViewModel>()
                .ForMember(b => b.Id, o => o.MapFrom(b => b.Id))
                .ForMember(b => b.FirstName, o => o.MapFrom(b => b.Name))
                .ForMember(b => b.LastName, o => o.MapFrom(b => b.SurName))
                .ForMember(b => b.DateOfBirth, o => o.MapFrom(b => b.DateOfBirth))
                .ForMember(b => b.DateOfDeath, o => o.MapFrom(b => b.DateOfDeath))
                .ReverseMap();
        }
    }
}