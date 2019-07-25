using System.Linq;
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
                Id = int.Parse(entity.Id),
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
            return new Book
            {
                Id = entity.Id,
                Name = entity.Name,
                Date = entity.Date,
                Summary = entity.Summary,
                Picture = entity.Picture,
                Authors = recurcive ? entity.Authors?.Select(x => x.Author.ToAuthor(false)).ToArray() : null
            };
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
                Authors = recurcive ? book.Authors?.Select(x => new BookAuthorEntity { Author = x.ToEntity(false) }).ToArray() : null
            };
        }

        public static Author ToAuthor(this AuthorEntity entity, bool recurcive = true)
        {
            return new Author
            {
                Id = entity.Id,
                Name = entity.Name,
                SurName = entity.SurName,
                DateOfBirth = entity.DateOfBirth,
                DateOfDeath = entity.DateOfDeath,
                Books = recurcive ? entity.Books?.Select(x => x.Book.ToBook(false)).ToArray() : null
            };
        }

        public static AuthorEntity ToEntity(this Author author, bool recurcive = true)
        {
            return new AuthorEntity
            {
                Id = author.Id,
                Name = author.Name,
                SurName = author.SurName,
                DateOfBirth = author.DateOfBirth,
                DateOfDeath = author.DateOfDeath,
                Books = recurcive ? author.Books?.Select(x => new BookAuthorEntity { Book = x.ToEntity(false) }).ToArray() : null
            };
        }
    }
}