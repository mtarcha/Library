using System;
using System.Collections.Generic;

namespace Library.Domain
{
    public class Author
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string SurName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public DateTime? DateOfDeath { get; set; }

        public IEnumerable<Book> Books { get; set; }
    }
}