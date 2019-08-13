using System;
using System.Collections.Generic;

namespace Library.Application.Common
{
    public class Book
    {
        public Book()
        {
            Authors = new List<Author>();
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public byte[] Picture { get; set; }

        public DateTime Date { get; set; }

        public ICollection<Author> Authors { get; set; }

        public string Summary { get; set; }

        public double? Rate { get; set; }
    }
}