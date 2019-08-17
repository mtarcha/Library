using System;
using System.Collections.Generic;
using System.Linq;

namespace Library.Application.Commands.Common
{
    public class Book
    {
        internal Book(Domain.Entities.Book book)
        {
            Id = book.Id;
            Name = book.Name;
            Picture = book.Picture;
            Date = book.Date;
            Summary = book.Summary;
            Rate = book.Rate;
            Authors = book.Authors.Select(x => new Author(x)).ToList();
        }

        public Guid Id { get; }

        public string Name { get; }

        public byte[] Picture { get; }

        public DateTime Date { get; }

        public ICollection<Author> Authors { get;  }

        public string Summary { get;  }

        public double? Rate { get;  }
    }
}