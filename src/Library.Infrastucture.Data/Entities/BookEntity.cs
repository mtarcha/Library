using System;
using System.Collections.Generic;

namespace Library.Infrastucture.Data.Entities
{
    public sealed class BookEntity : IEntity
    {
        public int Id { get; set; }

        public Guid ReferenceId { get; set; }

        public string Name { get; set; }

        public double Rate { get; set; }

        public DateTime Date { get; set; }

        public string Summary { get; set; }

        public byte[] Picture { get; set; }

        public List<BookAuthorEntity> Authors { get; set; }

        public List<BookRateEntity> Rates { get; set; }
    }
}