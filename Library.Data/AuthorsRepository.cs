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
        private readonly Repository<AuthorEntity> _internalRepository;

        public AuthorsRepository(LibraryContext ctx)
        {
            _ctx = ctx;
            ;
            _internalRepository = new Repository<AuthorEntity>(ctx);
        }

        public void Create(Author author)
        {
            _internalRepository.Create(author.ToEntity());
        }

        public Author GetById(int id)
        {
            return _ctx.Authors.Where(x => x.Id == id).Include(x => x.Books).ThenInclude(x => x.Book).Single().ToAuthor();
        }

        public void Update(Author author)
        {
            author.Books = null;
            _internalRepository.UpdateProperties(author.ToEntity());
        }

        public void Delete(int id)
        {
            _internalRepository.Delete(id);
        }

        public IQueryable<Author> Get(Predicate<Author> predicate)
        {
            return _internalRepository.Get(a => predicate(a.ToAuthor(true))).Include(x => x.Books).ThenInclude(x => x.Book).Select(x => x.ToAuthor(true));
        }
    }
}