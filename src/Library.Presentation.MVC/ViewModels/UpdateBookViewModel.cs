using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Library.Presentation.MVC.ViewModels
{
    public class UpdateBookViewModel
    {
        public UpdateBookViewModel()
        {
            Authors = new List<AuthorViewModel>();
        } 

        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public byte[] Picture { get; set; }

        [DataType(DataType.ImageUrl)]
        public IFormFile Avatar { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Summary { get; set; }

        public List<AuthorViewModel> Authors { get; set; }
    }
}