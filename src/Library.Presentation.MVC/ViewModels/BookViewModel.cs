using System;
using System.Collections.Generic;

namespace Library.Presentation.MVC.ViewModels
{
    public class BookViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public byte[] Picture { get; set; }

        public DateTime Date { get; set; }

        public IEnumerable<AuthorViewModel> Authors { get; set; }

        public string Summary { get; set; }

        public double Rate { get; set; }
    }
}