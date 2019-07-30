﻿using System.Linq;
using Library.Data.Entities;
using Library.Domain;

namespace Library.Data.Internal
{
    internal static class MappingExtensions
    {
        public static User ToUser(this UserEntity entity)
        {
            return new User
            {
                Id =entity.ReferenceId,
                Login = entity.UserName,
                DateOfBirth = entity.DateOfBirth
            };
        }

        public static UserEntity ToEntity(this User user)
        {
            return new UserEntity
            {
                Id = user.Id.ToString(),
                UserName = user.Login,
                DateOfBirth = user.DateOfBirth
            };
        }

        public static Book ToBook(this BookEntity entity, bool recurcive = true)
        {
            var book = new Book(entity.ReferenceId, entity.Name, entity.Date, entity.Summary, entity.Picture);
            if (recurcive && entity.Authors != null)
            {
                foreach (var author in entity.Authors)
                {
                    book.AddAuthor(author.Author.ToAuthor(false));
                }
            }

            if (entity.Rates != null)
            {
                book.SetRates(entity.Rates.Select(x => x.ToRate()));

            }

            return book;
        }

        public static BookRateEntity ToEntity(this BookRate rate, BookEntity book)
        {
            return new BookRateEntity
            {
                ReferenceId = rate.Id,
                User = rate.User.ToEntity(),
                Rate = rate.Rate,
                Book = book
            };
        }

        public static BookRate ToRate(this BookRateEntity entity)
        {
            return new BookRate(entity.ReferenceId, entity.User.ToUser(), entity.Rate);
        }

        public static BookEntity ToEntity(this Book book, bool recurcive = true)
        {
            return new BookEntity
            {
                ReferenceId = book.Id,
                Name = book.Name,
                Date = book.Date,
                Summary = book.Summary,
                Picture = book.Picture,
                Authors = recurcive ? book.Authors?.Select(x => new BookAuthorEntity { Author = x.ToEntity(false) }).ToList() : null
            };
        }

        public static Author ToAuthor(this AuthorEntity entity, bool recurcive = true)
        {
            var author = new Author(entity.ReferenceId, entity.Name, entity.SurName, new LifePeriod(entity.DateOfBirth, entity.DateOfDeath));
            if (entity.Books != null && recurcive)
            {
                foreach (var book in entity.Books)
                {
                    author.AddBook(book.Book.ToBook(false));
                }
            }

            return author;
        }

        public static AuthorEntity ToEntity(this Author author, bool recurcive = true)
        {
            return new AuthorEntity
            {
                ReferenceId = author.Id,
                Name = author.Name,
                SurName = author.SurName,
                DateOfBirth = author.LifePeriod.DateOfBirth,
                DateOfDeath = author.LifePeriod.DateOfDeath,
                Books = recurcive ? author.Books?.Select(x => new BookAuthorEntity { Book = x.ToEntity(false) }).ToList() : null
            };
        }
    }
}