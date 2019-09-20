using System;
using System.Collections.Generic;
using Library.Domain.Entities;

namespace Library.Domain
{
    public interface IEntityFactory
    {
        Book CreateBook(string name, DateTime date, string summary);

        Book CreateBook(string name, DateTime date, string summary, byte[] picture);

        Book CreateBook(Guid id, string name, DateTime date, string summary, byte[] picture);

        Book CreateBook(Guid id, string name, DateTime date, string summary, byte[] picture, IEnumerable<BookRate> rates);

        BookRate CreateBookRate(User user, int rate);

        BookRate CreateBookRate(Guid id, User user, int rate);

        Author CreateAuthor(string name, string surName, LifePeriod lifePeriod);

        Author CreateAuthor(Guid id, string name, string surName, LifePeriod lifePeriod);

        User CreateUser(string userName, DateTime dateOfBirth);
        
        User CreateUser(Guid id, string userName, DateTime dateOfBirth, IEnumerable<Book> favoriteBooks, IEnumerable<Book> recommendedBooks, IEnumerable<User> favoriteReviewers);
    }
}