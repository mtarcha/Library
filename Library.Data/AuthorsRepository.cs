using System;
using Library.Domain;
using System.Linq;
using Library.Data.Entities;
using Library.Data.Internal;
using Microsoft.EntityFrameworkCore;

namespace Library.Data
{
    public sealed class AuthorsRepository : IAuthorsRepository
    {
        private readonly LibraryContext _ctx;
        private readonly Repository<BookEntity> _booksRepository;
        private readonly Repository<AuthorEntity> _authorsRepository;

        public AuthorsRepository(LibraryContext ctx)
        {
            _ctx = ctx;
            _booksRepository = new Repository<BookEntity>(ctx);
            _authorsRepository = new Repository<AuthorEntity>(ctx);
        }

        public void Create(Author author)
        {
            _authorsRepository.Create(author.ToEntity());
        }

        public Author GetById(int id)
        {
            return _ctx.Authors.Where(x => x.Id == id).Include(x => x.Books).ThenInclude(x => x.Book).Single().ToAuthor();
        }

        public void Update(Author author)
        {
            var books = author.Books;

            author.Books = null;
            _authorsRepository.UpdateProperties(author.ToEntity());

            if (books != null)
            {
                foreach (var book in books)
                {
                    _booksRepository.UpdateProperties(book.ToEntity(false));
                }
            }
        }

        public void Delete(int id)
        {
            _authorsRepository.Delete(id);
        }

        public IQueryable<Author> Get(Predicate<Author> predicate)
        {
            return _authorsRepository.Get(a => predicate(a.ToAuthor(true))).Include(x => x.Books).ThenInclude(x => x.Book).Select(x => x.ToAuthor(true));
        }
    }
}