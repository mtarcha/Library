using System.Collections.Generic;
using System.Linq;
using Library.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.Data
{
    class LibraryRepository : ILibraryRepository
    {
        private readonly LibraryContext _ctx;

        public LibraryRepository(LibraryContext ctx)
        {
            _ctx = ctx;
        }

        public IEnumerable<Book> GetAllBooks()
        {
            return _ctx.Books.Include(x => x.Authors).ThenInclude(x => x.Author).ToArray();
        }

        public IEnumerable<Book> GetBooksByName(string name)
        {
            return _ctx.Books.Where(x => x.Name.Contains(name)).Include(x => x.Authors).ThenInclude(x => x.Author).ToArray();
        }

        public IEnumerable<Book> GetBooksByAuthorName(string name)
        {
            return _ctx.Books.Where(x => x.Authors.Any(a => a.Author.Name.Contains(name) || a.Author.SurName.Contains(name))).Include(x => x.Authors).ThenInclude(x => x.Author).ToArray();
        }

        public IEnumerable<Author> GetAllAuthors()
        {
            return _ctx.Author.Include(x => x.Books).ThenInclude(x => x.Book).ToArray();
        }

        public Author GetAuthor(int id)
        {
            return _ctx.Author.Where(x => x.Id == id).Include(x => x.Books).ThenInclude(x => x.Book).Single();
        }

        public void AddNewAuthor(Author authorModel)
        {
            _ctx.Author.Add(authorModel);
            _ctx.SaveChanges();
        }

        public void AddNewBook(Book bookModel)
        {
            _ctx.Books.Add(bookModel);
            _ctx.SaveChanges();
        }
    }
}