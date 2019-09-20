using AutoMapper;
using Library.Api.ViewModels;
using Library.Application.Commands.CreateBook;
using Library.Application.Commands.RegisterUser;
using Library.Application.Commands.UpdateBook;

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
                
            CreateMap<CreateBookViewModel, CreateBookCommand>()
                .ForMember(b => b.Date, o => o.MapFrom(b => b.Date))
                .ForMember(b => b.Name, o => o.MapFrom(b => b.Name))
                .ForMember(b => b.Summary, o => o.MapFrom(b => b.Summary))
                .ForMember(b => b.Picture, o => o.MapFrom(b => b.Picture));

            CreateMap<AddUserViewModel, AddUserCommand>()
                .ForMember(r => r.UserName, o => o.MapFrom(r => r.UserName))
                .ForMember(r => r.DateOfBirth, o => o.MapFrom(r => r.DateOfBirth))
                .ForMember(r => r.PhoneNumber, o => o.MapFrom(r => r.PhoneNumber));
        }
    }
}