using System;
using System.Threading.Tasks;
using Library.Data.Entities;
using Microsoft.EntityFrameworkCore.Internal;

namespace Library.Data.Seeders
{
    public class LibrarySeeder : ISeeder
    {
        private readonly LibraryContext _ctx;

        public LibrarySeeder(LibraryContext ctx)
        {
            _ctx = ctx;
        }

        public Task Seed()
        {
            return Task.Run(() =>
            {
                _ctx.Database.EnsureCreated();

                if (!_ctx.Author.Any() || !_ctx.Books.Any())
                {
                    var ivanko = new Author
                    {
                        Name = "Ivan",
                        SurName = "Ivchenko",
                        DateOfBirth = new DateTime(1988, 1, 11)
                    };

                    var slavko = new Author
                    {
                        Name = "Slavka",
                        SurName = "Tarcha",
                        DateOfBirth = new DateTime(1993, 5, 5)
                    };

                    var book1 = new Book
                    {
                        Name = "Sherlok",
                        Date = DateTime.Now,
                        Summary = "some sumamry",
                    };

                    book1.Authors = new[] { new BookAuthor { Book = book1, Author = ivanko } };

                    var book2 = new Book
                    {
                        Name = "Sherlok2",
                        Date = DateTime.Now,
                        Summary = "some sumamry"
                    };

                    book2.Authors = new[]
                    {
                        new BookAuthor {Book = book2, Author = ivanko},
                        new BookAuthor {Book = book2, Author = slavko}
                    };

                    _ctx.Books.AddRange(new[] { book1, book2 });
                    _ctx.SaveChanges();
                }
            });

        }
    }
}