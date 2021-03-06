﻿using System.IO;
using AutoMapper;
using Library.Presentation.MVC.Models;
using Library.Presentation.MVC.ViewModels;
using Microsoft.AspNetCore.Http;

namespace Library.Presentation.MVC.Utility
{
    public class ViewModelsMapper : Profile
    {
        public ViewModelsMapper()
        {
            CreateMap<Book, BookViewModel>()
                .ForMember(b => b.Id, o => o.MapFrom(b => b.Id))
                .ForMember(b => b.Date, o => o.MapFrom(b => b.Date))
                .ForMember(b => b.Name, o => o.MapFrom(b => b.Name))
                .ForMember(b => b.Summary, o => o.MapFrom(b => b.Summary))
                .ForMember(b => b.Picture, o => o.MapFrom(b => b.Picture))
                .ForMember(b => b.Rate, o => o.MapFrom(b => b.Rate))
                .ReverseMap();

            CreateMap<Book, UpdateBookViewModel>()
               .ForMember(b => b.Id, o => o.MapFrom(b => b.Id))
               .ForMember(b => b.Date, o => o.MapFrom(b => b.Date))
               .ForMember(b => b.Name, o => o.MapFrom(b => b.Name))
               .ForMember(b => b.Summary, o => o.MapFrom(b => b.Summary))
               .ForMember(b => b.Picture, o => o.MapFrom(b => b.Picture))
               .ForMember(b => b.Avatar, o => o.Ignore())
               .ForMember(b => b.Authors, o => o.MapFrom(b => b.Authors));
            
            CreateMap<UpdateBookViewModel, UpdateBookModel>()
                .ForMember(b => b.Id, o => o.MapFrom(b => b.Id))
                .ForMember(b => b.Date, o => o.MapFrom(b => b.Date))
                .ForMember(b => b.Name, o => o.MapFrom(b => b.Name))
                .ForMember(b => b.Summary, o => o.MapFrom(b => b.Summary))
                .ForMember(b => b.Picture, o => o.MapFrom((b, c, d) =>
                {
                    return b.Avatar?.Length > 0 ? ReadImageData(b.Avatar) : b.Picture;
                }));
            
            CreateMap<CreateBookViewModel, CreateBookModel>()
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

            CreateMap<UpdateAuthorModel, AuthorViewModel>()
                .ForMember(b => b.Id, o => o.MapFrom(b => b.Id))
                .ForMember(b => b.FirstName, o => o.MapFrom(b => b.FirstName))
                .ForMember(b => b.LastName, o => o.MapFrom(b => b.LastName))
                .ForMember(b => b.DateOfBirth, o => o.MapFrom(b => b.DateOfBirth))
                .ForMember(b => b.DateOfDeath, o => o.MapFrom(b => b.DateOfDeath))
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