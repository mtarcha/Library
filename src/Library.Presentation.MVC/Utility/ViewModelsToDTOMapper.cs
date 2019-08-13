using System.IO;
using AutoMapper;
using Library.Application.Commands.LoginUser;
using Library.Application.Commands.RegisterUser;
using Library.Application.Common;
using Library.Presentation.MVC.ViewModels;
using Microsoft.AspNetCore.Http;

namespace Library.Presentation.MVC.Utility
{
    public class ViewModelsToDTOMapper : Profile
    {
        public ViewModelsToDTOMapper()
        {
            CreateMap<Book, BookViewModel>()
                .ForMember(b => b.Id, o => o.MapFrom(b => b.Id))
                .ForMember(b => b.Date, o => o.MapFrom(b => b.Date))
                .ForMember(b => b.Name, o => o.MapFrom(b => b.Name))
                .ForMember(b => b.Summary, o => o.MapFrom(b => b.Summary))
                .ForMember(b => b.Picture, o => o.MapFrom(b => b.Picture))
                .ForMember(b => b.Rate, o => o.MapFrom(b => b.Rate))
                .ReverseMap();

            CreateMap<Book, EditBookViewModel>()
               .ForMember(b => b.Id, o => o.MapFrom(b => b.Id))
               .ForMember(b => b.Date, o => o.MapFrom(b => b.Date))
               .ForMember(b => b.Name, o => o.MapFrom(b => b.Name))
               .ForMember(b => b.Summary, o => o.MapFrom(b => b.Summary))
               .ForMember(b => b.Picture, o => o.MapFrom(b => b.Picture))
               .ForMember(b => b.Avatar, o => o.Ignore())
               .ForMember(b => b.Authors, o => o.MapFrom(b => b.Authors));
            
            CreateMap<EditBookViewModel, Book>()
                .ForMember(b => b.Id, o => o.MapFrom(b => b.Id))
                .ForMember(b => b.Date, o => o.MapFrom(b => b.Date))
                .ForMember(b => b.Name, o => o.MapFrom(b => b.Name))
                .ForMember(b => b.Summary, o => o.MapFrom(b => b.Summary))
                .ForMember(b => b.Picture, o => o.MapFrom((b, c, d) =>
                {
                    return b.Avatar?.Length > 0 ? ReadImageData(b.Avatar) : b.Picture;
                }));
            
            CreateMap<CreateBookViewModel, Book>()
                .ForMember(b => b.Date, o => o.MapFrom(b => b.Date))
                .ForMember(b => b.Name, o => o.MapFrom(b => b.Name))
                .ForMember(b => b.Summary, o => o.MapFrom(b => b.Summary))
                .ForMember(b => b.Picture, o => o.MapFrom((b, c, d) => ReadImageData(b.Avatar)));


            CreateMap<Author, AuthorViewModel>()
              .ForMember(b => b.Id, o => o.MapFrom(b => b.Id))
              .ForMember(b => b.FirstName, o => o.MapFrom(b => b.FirstName))
              .ForMember(b => b.LastName, o => o.MapFrom(b => b.LastName))
              .ForMember(b => b.DateOfBirth, o => o.MapFrom(b => b.DateOfBirth))
              .ForMember(b => b.DateOfDeath, o => o.MapFrom(b => b.DateOfDeath))
              .ReverseMap();

            CreateMap<RegisterUserCommand, RegisterViewModel>()
                .ForMember(r => r.UserName, o => o.MapFrom(r => r.UserName))
                .ForMember(r => r.DateOfBirth, o => o.MapFrom(r => r.DateOfBirth))
                .ForMember(r => r.Password, o => o.MapFrom(r => r.Password))
                .ForMember(r => r.PhoneNumber, o => o.MapFrom(r => r.PhoneNumber))
                .ForMember(r => r.PasswordConfirm, o => o.Ignore())
                .ReverseMap();

            CreateMap<LoginUserCommand, LoginViewModel>()
                .ForMember(r => r.UserName, o => o.MapFrom(r => r.UserName))
                .ForMember(r => r.RememberMe, o => o.MapFrom(r => r.RememberMe))
                .ForMember(r => r.Password, o => o.MapFrom(r => r.Password))
                .ReverseMap();
        }

        private byte[] ReadImageData(IFormFile formFile)
        {
            byte[] imageData = null;
            using (var binaryReader = new BinaryReader(formFile.OpenReadStream()))
            {
                imageData = binaryReader.ReadBytes((int)formFile.Length);
            }

            return imageData;
        }
    }
}