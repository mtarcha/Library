using System.IO;
using AutoMapper;
using Library.Data.Entities;

namespace Library.ViewModels
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
                .ForMember(b => b.Picture, o => o.MapFrom(b => b.Avatar));
            //.ForMember(b => b.Rate, o => o.MapFrom(b => b.));

            CreateMap<Book, EditBookViewModel>()
               .ForMember(b => b.Id, o => o.MapFrom(b => b.Id))
               .ForMember(b => b.Date, o => o.MapFrom(b => b.Date))
               .ForMember(b => b.Name, o => o.MapFrom(b => b.Name))
               .ForMember(b => b.Summary, o => o.MapFrom(b => b.Summary))
               .ForMember(b => b.Picture, o => o.MapFrom(b => b.Avatar))
               .ForMember(b => b.Avatar, o => o.Ignore());

            CreateMap<CreateBookViewModel, Book>()
               .ForMember(b => b.Date, o => o.MapFrom(b => b.Date))
               .ForMember(b => b.Name, o => o.MapFrom(b => b.Name))
               .ForMember(b => b.Summary, o => o.MapFrom(b => b.Summary))
               .ForMember(b => b.Avatar, o => o.MapFrom((b, c, d) =>
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
              .ForMember(b => b.Avatar, o => o.MapFrom((b, c, d) =>
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
                  
                  return null;
              }));

            CreateMap<Author, AuthorViewModel>()
                .ForMember(b => b.FirstName, o => o.MapFrom(b => b.Name))
                .ForMember(b => b.LastName, o => o.MapFrom(b => b.SurName))
                .ForMember(b => b.DateOfBirth, o => o.MapFrom(b => b.DateOfBirth))
                .ForMember(b => b.DateOfDeath, o => o.MapFrom(b => b.DateOfDeath))
                .ReverseMap();

            CreateMap<BookAuthor, AuthorViewModel>()
               .ForMember(b => b.FirstName, o => o.MapFrom(b => b.Author.Name))
               .ForMember(b => b.LastName, o => o.MapFrom(b => b.Author.SurName))
               .ForMember(b => b.DateOfBirth, o => o.MapFrom(b => b.Author.DateOfBirth))
               .ForMember(b => b.DateOfDeath, o => o.MapFrom(b => b.Author.DateOfDeath))
               .ReverseMap();

            CreateMap<BookAuthor, BookViewModel>()
                .ForMember(b => b.Date, o => o.MapFrom(b => b.Book.Date))
                .ForMember(b => b.Name, o => o.MapFrom(b => b.Book.Name))
                .ForMember(b => b.Summary, o => o.MapFrom(b => b.Book.Summary))
                .ForMember(b => b.Picture, o => o.MapFrom(b => b.Book.Avatar));
        }
    }
}