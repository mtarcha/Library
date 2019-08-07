using System;
using System.Collections.Generic;

namespace Library.Business.DTO
{
    public class Book
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public byte[] Picture { get; set; }

        public DateTime Date { get; set; }

        public IEnumerable<Author> Authors { get; set; }

        public string Summary { get; set; }

        public double Rate { get; set; }
    }
}