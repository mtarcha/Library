using System;
using System.Collections.Generic;

namespace Library.Data.Entities
{
    public class Book
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime Date { get; set; }

        public string Summary { get; set; }

        public IEnumerable<BookAuthor> Authors { get; set; }
    }
}