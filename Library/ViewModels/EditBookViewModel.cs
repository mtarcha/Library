using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Library.Presentation.ViewModels
{
    public class EditBookViewModel
    {
        public EditBookViewModel()
        {
            Authors = new List<AuthorViewModel>();
        } 

        public int Id { get; set; }

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

        [Required]
        public List<AuthorViewModel> Authors { get; set; }
    }
}