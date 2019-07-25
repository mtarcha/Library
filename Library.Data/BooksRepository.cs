using System;
using System.Linq;
using System.Linq.Expressions;
using Library.Data.Entities;
using Library.Data.Internal;
using Library.Domain;
using Microsoft.EntityFrameworkCore;

namespace Library.Data
{
    public sealed class BooksRepository : IBooksRepository
    {
        private readonly LibraryContext _ctx;
        private readonly Repository<BookEntity> _internalRepository;

        public BooksRepository(LibraryContext ctx)
        {
            _ctx = ctx;
            _internalRepository = new Repository<BookEntity>(ctx);
        }

        public void Create(Book book)
        {
            _internalRepository.Create(book.ToEntity());
        }

        public Book GetById(int id)
        {
            return _ctx.Books.Where(x => x.Id == id).Include(x => x.Authors).ThenInclude(x => x.Author).Single().ToBook();
        }

        public void Update(Book book)
        {
            book.Authors = null;
            _internalRepository.UpdateProperties(book.ToEntity());
        }

        public void Delete(int id)
        {
            _internalRepository.Delete(id);
        }

        public IQueryable<Book> Get(Predicate<Book> predicate)
        {
            return _internalRepository.Get(b => predicate(b.ToBook(true))).Include(x => x.Authors).ThenInclude(x => x.Author).Select(x => x.ToBook(true));
        }
    }
}