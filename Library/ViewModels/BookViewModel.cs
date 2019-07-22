using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Library.ViewModels
{
    public class BookViewModel
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public byte[] Picture { get; set; }

        [Required]
        [DataType(DataType.ImageUrl)]
        public IFormFile Avatar { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        //[Required]
        public IEnumerable<AuthorViewModel> Authors { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Summary { get; set; }

        public int Rate { get; set; }
    }
}