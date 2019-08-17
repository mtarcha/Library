using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Library.Api.ViewModels
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

        [Required]
        public byte[] Picture { get; set; }
        
        [Required]
        public DateTime Date { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Summary { get; set; }

        public List<AuthorViewModel> Authors { get; set; }
    }
}