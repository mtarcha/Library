using System;
using System.Collections.Generic;

namespace Library.Domain
{
    public class Book
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime Date { get; set; }

        public string Summary { get; set; }

        public byte[] Picture { get; set; }

        public IEnumerable<Author> Authors { get; set; }
    }
}
