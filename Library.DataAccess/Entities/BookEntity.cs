using System;
using System.Collections.Generic;

namespace Library.DataAccess.Entities
{
    internal sealed class BookEntity : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime Date { get; set; }

        public string Summary { get; set; }

        public byte[] Picture { get; set; }

        public IEnumerable<BookAuthorEntity> Authors { get; set; }
    }
}