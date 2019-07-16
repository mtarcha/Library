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

        public IEnumerable<Book> GetAllBooksIncludingAuthors()
        {
            return _ctx.Books.Include(x => x.Authors).ThenInclude(x => x.Author).ToArray();
        }
    }
}