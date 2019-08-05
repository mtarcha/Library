using System;
using System.Collections.Generic;
using Library.Domain.Common;

namespace Library.Domain
{
    public class Author : Entity<Guid>, IAggregateRoot
    {
        private readonly List<Book> _books;

        internal Author(EventDispatcher eventDispatcher, string name, string surName, LifePeriod lifePeriod)
            : this(Guid.NewGuid(), eventDispatcher, name, surName, lifePeriod)
        {
        }

        internal Author(Guid id, EventDispatcher eventDispatcher, string name, string surName, LifePeriod lifePeriod)
            : base(id, eventDispatcher)
        {
            Name = name;
            SurName = surName;
            LifePeriod = lifePeriod;
            _books = new List<Book>();
        }

        public string Name { get; }

        public string SurName { get; }

        public LifePeriod LifePeriod { get; }

        public IReadOnlyList<Book> Books
        {
            get { return _books; }
        }

        public void AddBook(Book book)
        {
            if (!_books.Exists(x => x.Id == book.Id))
            {
                _books.Add(book);
                book.AddAuthor(this);
            }
        }
    }
}