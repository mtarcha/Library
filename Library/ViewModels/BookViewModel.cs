using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Library.ViewModels
{
    public class BookViewModel
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public IEnumerable<AuthorViewModel> Authors { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Summary { get; set; }

        public int Rate { get; set; }
    }
}