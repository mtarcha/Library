using System.Collections.Generic;
using System.Linq;
using Library.Domain;
using Library.Infrastructure.Data.Entities;

namespace Library.Infrastructure.Data.Internal
{
    internal static class MappingExtensions
    {
        public static User ToUser(this UserEntity entity, IEntityFactory entityFactory)
        {
            return entityFactory.CreateUser(entity.Id, entity.UserName, entity.DateOfBirth, null);
        }

        public static UserEntity ToEntity(this User user)
        {
            return new UserEntity
            {
                Id = user.Id,
                UserName = user.UserName,
                DateOfBirth = user.DateOfBirth,
            };
        }

        public static Book ToBook(this BookEntity entity, IEntityFactory entityFactory, bool recurcive = true)
        {
            var rates = entity.Rates != null ? entity.Rates.Select(x => x.ToRate(entityFactory)) : new List<BookRate>();
            var book = entityFactory.CreateBook(entity.Id, entity.Name, entity.Date, entity.Summary, entity.Picture, rates);
            if (recurcive && entity.Authors != null)
            {
                foreach (var author in entity.Authors)
                {
                    book.AddAuthor(author.Author.ToAuthor(entityFactory, false));
                }
            }

            return book;
        }

        public static BookRateEntity ToEntity(this BookRate rate, BookEntity book)
        {
            return new BookRateEntity
            {
                Id = rate.Id,
                User = rate.User.ToEntity(),
                Rate = rate.Rate,
                Book = book
            };
        }

        public static BookRate ToRate(this BookRateEntity entity, IEntityFactory entityFactory)
        {
            return entityFactory.CreateBookRate(entity.Id, entity.User.ToUser(entityFactory), entity.Rate);
        }

        public static BookEntity ToEntity(this Book book, bool recurcive = true)
        {
            return new BookEntity
            {
                Id = book.Id,
                Name = book.Name,
                Date = book.Date,
                Summary = book.Summary,
                Picture = book.Picture,
                Authors = recurcive ? book.Authors?.Select(x => new BookAuthorEntity { Author = x.ToEntity(false) }).ToList() : null
            };
        }

        public static Author ToAuthor(this AuthorEntity entity, IEntityFactory entityFactory, bool recurcive = true)
        {
            var author = entityFactory.CreateAuthor(entity.Id, entity.Name, entity.SurName, new LifePeriod(entity.DateOfBirth, entity.DateOfDeath));
            if (entity.Books != null && recurcive)
            {
                foreach (var book in entity.Books)
                {
                    author.AddBook(book.Book.ToBook(entityFactory, false));
                }
            }

            return author;
        }

        public static AuthorEntity ToEntity(this Author author, bool recurcive = true)
        {
            var entity = new AuthorEntity
            {
                Id = author.Id,
                Name = author.Name,
                SurName = author.SurName,
                DateOfBirth = author.LifePeriod.DateOfBirth,
                DateOfDeath = author.LifePeriod.DateOfDeath
            };

            entity.Books = recurcive
                ? author.Books?.Select(x => new BookAuthorEntity {Book = x.ToEntity(false), Author = entity}).ToList()
                : null;

            return entity;
        }
    }
}