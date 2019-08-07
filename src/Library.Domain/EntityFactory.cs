using System;
using System.Collections.Generic;
using Library.Domain.Common;

namespace Library.Domain
{
    public class EntityFactory : IEntityFactory
    {
        private readonly IEventDispatcher _dispatcher;

        public EntityFactory(IEventDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public Book CreateBook(string name, DateTime date, string summary)
        {
            return new Book(_dispatcher, name, date, summary);
        }

        public Book CreateBook(string name, DateTime date, string summary, byte[] picture)
        {
            return new Book(_dispatcher, name, date, summary, picture);
        }

        public Book CreateBook(Guid id, string name, DateTime date, string summary, byte[] picture, IEnumerable<BookRate> rates)
        {
            return new Book(_dispatcher, id, name, date, summary, picture, rates);
        }

        public BookRate CreateBookRate(User user, int rate)
        {
            return new BookRate(_dispatcher, user, rate);
        }

        public BookRate CreateBookRate(Guid id, User user, int rate)
        {
            return new BookRate(id, _dispatcher, user, rate);
        }

        public Author CreateAuthor(string name, string surName, LifePeriod lifePeriod)
        {
            return new Author(_dispatcher, name, surName, lifePeriod);
        }

        public Author CreateAuthor(Guid id, string name, string surName, LifePeriod lifePeriod)
        {
            return new Author(id, _dispatcher, name, surName, lifePeriod);
        }

        public User CreateUser(string userName, DateTime dateOfBirth)
        {
            return new User(_dispatcher, userName, dateOfBirth);
        }

        public User CreateUser(string userName, DateTime dateOfBirth, Role role)
        {
            return new User(_dispatcher, userName, dateOfBirth, role);
        }

        public User CreateUser(Guid id, string userName, DateTime dateOfBirth, Role role)
        {
            return new User(id, _dispatcher, userName, dateOfBirth, role);
        }
    }
}