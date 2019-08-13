using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Library.Domain;
using Library.Infrastructure.Data.Entities;
using Library.Infrastructure.Data.Internal;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Data
{
    public sealed class AuthorsRepository : IAuthorsRepository
    {
        private readonly LibraryContext _ctx;
        private readonly IEntityFactory _entityFactory;

        public AuthorsRepository(LibraryContext ctx, IEntityFactory entityFactory)
        {
            _ctx = ctx;
            _entityFactory = entityFactory;
        }

        public async Task CreateAsync(Author author, CancellationToken token)
        {
            var entity = author.ToEntity();
            await _ctx.Authors.AddAsync(entity, token);
        }

        public async Task<Author> GetByIdAsync(Guid id, CancellationToken token)
        {
            var entity = await _ctx.Authors
                .Include(x => x.Books).ThenInclude(x => x.Book)
                .SingleAsync(x => x.Id == id, token);

            return entity.ToAuthor(_entityFactory);
        }

        public async Task UpdateAsync(Author author, CancellationToken token)
        {
            var entity = await _ctx.Authors
                .Include(x => x.Books).ThenInclude(x => x.Book)
                .SingleAsync(x => x.Id == author.Id, token);

            entity.Name = author.Name;
            entity.SurName = author.SurName;
            entity.DateOfBirth = author.LifePeriod.DateOfBirth;
            entity.DateOfDeath = author.LifePeriod.DateOfDeath;

            if (author.Books != null)
            {
                var savedBooks = entity.Books.ToList();
                var currentBooks = author.Books.ToList();

                var updatedBooks = savedBooks.Where(x => currentBooks.Exists(b => b.Id == x.Book.Id)).ToList();
                var newBooks = currentBooks.Where(x => !savedBooks.Exists(b => b.Book.Id == x.Id)).ToList();
                var deletedBooks = savedBooks.Except(updatedBooks);

                foreach (var deletedBook in deletedBooks)
                {
                    entity.Books.Remove(deletedBook);
                }

                foreach (var updatedBook in updatedBooks)
                {
                    var update = currentBooks.Single(x => x.Id == updatedBook.Book.Id);
                    updatedBook.Book.Name = update.Name;
                    updatedBook.Book.Date = update.Date;
                    updatedBook.Book.Picture = update.Picture;
                }

                foreach (var newBook in newBooks)
                {
                    entity.Books.Add(new BookAuthorEntity { Book = newBook.ToEntity() });
                }
            }
        }

        public async Task DeleteAsync(Guid id, CancellationToken token)
        {
            var author = await _ctx.Authors.SingleAsync(x => x.Id == id, token);
            _ctx.Remove(author);
        }
    }
}