using AutoMapper;
using Library.Api.ViewModels;
using Library.Application.Commands.AddUser;
using Library.Application.Commands.CreateBook;
using Library.Application.Commands.UpdateBook;
using Library.Messaging.Contracts;

namespace Library.Api.Utility
{
    public class ViewModelsMapper : Profile
    {
        public ViewModelsMapper()
        {
            CreateMap<UpdateBookViewModel, UpdateBookCommand>()
                .ForMember(b => b.Id, o => o.MapFrom(b => b.Id))
                .ForMember(b => b.Date, o => o.MapFrom(b => b.Date))
                .ForMember(b => b.Name, o => o.MapFrom(b => b.Name))
                .ForMember(b => b.Summary, o => o.MapFrom(b => b.Summary))
                .ForMember(b => b.Picture, o => o.MapFrom(b => b.Picture));

            CreateMap<AuthorViewModel, UpdateAuthor>()
                .ForMember(b => b.Id, o => o.MapFrom(b => b.Id))
                .ForMember(b => b.FirstName, o => o.MapFrom(b => b.FirstName))
                .ForMember(b => b.LastName, o => o.MapFrom(b => b.LastName))
                .ForMember(b => b.DateOfBirth, o => o.MapFrom(b => b.DateOfBirth))
                .ForMember(b => b.DateOfDeath, o => o.MapFrom(b => b.DateOfDeath));

            CreateMap<CreateBookViewModel, CreateBookCommand>()
                .ForMember(b => b.Date, o => o.MapFrom(b => b.Date))
                .ForMember(b => b.Name, o => o.MapFrom(b => b.Name))
                .ForMember(b => b.Summary, o => o.MapFrom(b => b.Summary))
                .ForMember(b => b.Picture, o => o.MapFrom(b => b.Picture));

            CreateMap<AddUserViewModel, AddUserCommand>()
                .ForMember(r => r.UserName, o => o.MapFrom(r => r.UserName))
                .ForMember(r => r.DateOfBirth, o => o.MapFrom(r => r.DateOfBirth))
                .ForMember(r => r.PhoneNumber, o => o.MapFrom(r => r.PhoneNumber));

            CreateMap<NewUserRegistered, AddUserCommand>()
                .ForMember(r => r.UserName, o => o.MapFrom(r => r.UserName))
                .ForMember(r => r.DateOfBirth, o => o.MapFrom(r => r.DateOfBirth))
                .ForMember(r => r.PhoneNumber, o => o.MapFrom(r => r.PhoneNumber));
        }
    }
}