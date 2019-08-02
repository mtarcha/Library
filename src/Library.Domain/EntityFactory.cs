using System;
using Library.Domain.Events;

namespace Library.Domain
{
    public class EntityFactory
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

        public Book CreateBook(Guid id, string name, DateTime date, string summary, byte[] picture)
        {
            return new Book(_dispatcher, id, name, date, summary, picture);
        }

        public Author CreateAuthor(string name, string surName, LifePeriod lifePeriod)
        {
            return new Author(name, surName, lifePeriod);
        }

        public Author CreateAuthor(Guid id, string name, string surName, LifePeriod lifePeriod)
        {
            return new Author(id, name, surName, lifePeriod);
        }

        public User CreateUser(string userName, DateTime dateOfBirth)
        {
            return new User(userName, dateOfBirth);
        }

        public User CreateUser(string userName, DateTime dateOfBirth, Role role)
        {
            return new User(userName, dateOfBirth, role);
        }

        public User CreateUser(Guid id, string userName, DateTime dateOfBirth, Role role)
        {
            return new User(id, userName, dateOfBirth, role);
        }
    }
}