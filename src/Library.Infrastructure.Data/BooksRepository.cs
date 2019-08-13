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
    public sealed class BooksRepository : IBooksRepository
    {
        private readonly LibraryContext _ctx;
        private readonly IEntityFactory _entityFactory;

        public BooksRepository(LibraryContext ctx, IEntityFactory entityFactory)
        {
            _ctx = ctx;
            _entityFactory = entityFactory;
        }

        public async Task CreateAsync(Book book, CancellationToken token)
        {
            var entity = book.ToEntity();
            await _ctx.Books.AddAsync(entity, token);
        }

        public async Task<Book> GetByIdAsync(Guid id, CancellationToken token)
        {
            var entity = await _ctx.Books
                .Include(x => x.Authors).ThenInclude(x => x.Author)
                .Include(x => x.Rates).ThenInclude(x => x.User)
                .SingleAsync(x => x.Id == id, token);

            return entity.ToBook(_entityFactory);
        }

        public async Task UpdateAsync(Book book, CancellationToken token)
        {
            var entity = await _ctx.Books
                .Include(x => x.Authors).ThenInclude(x => x.Author)
                .Include(x => x.Rates)
                .SingleAsync(x => x.Id == book.Id, token);

            entity.Name = book.Name;
            entity.Date = book.Date;
            entity.Summary = book.Summary;
            entity.Picture = book.Picture;

            if (book.Authors != null)
            {
                var savedAuthors = entity.Authors.ToList();
                var currentAuthors = book.Authors.ToList();

                var updatedAuthors = savedAuthors.Where(x => currentAuthors.Exists(b => b.Id == x.Author.Id)).ToList();
                var newAuthors = currentAuthors.Where(x => !savedAuthors.Exists(b => b.Author.Id == x.Id)).ToList();
                var deletedAuthors = savedAuthors.Except(updatedAuthors);

                foreach (var deletedAuthor in deletedAuthors)
                {
                    entity.Authors.Remove(deletedAuthor);
                }

                foreach (var updatedAuthor in updatedAuthors)
                {
                    var update = currentAuthors.Single(x => x.Id == updatedAuthor.Author.Id);
                    updatedAuthor.Author.Name = update.Name;
                    updatedAuthor.Author.SurName = update.SurName;
                    updatedAuthor.Author.DateOfBirth = update.LifePeriod.DateOfBirth;
                    updatedAuthor.Author.DateOfDeath = update.LifePeriod.DateOfDeath;
                }

                foreach (var newAuthor in newAuthors)
                {
                    entity.Authors.Add(new BookAuthorEntity { Author = newAuthor.ToEntity() });
                }
            }

            if (book.Rates != null && book.Rates.Any())
            {
                if (!book.Rate.HasValue)
                {
                    throw new ArgumentException("Book has rates but no evaluated rate");
                }

                entity.Rate = book.Rate.Value;
                var saved = entity.Rates.ToList();
                var current = book.Rates.ToList();

                var updatedRates = saved.Where(x => current.Exists(r => r.Id == x.Id)).ToList();
                var newRates = current.Where(x => !saved.Exists(b => b.Id == x.Id)).ToList();

                foreach (var rate in updatedRates)
                {
                    var update = current.Single(x => x.Id == rate.Id);
                    rate.Rate = update.Rate;
                }

                foreach (var rate in newRates)
                {
                    var user = _ctx.Users.Single(x => x.Id == rate.User.Id);
                    entity.Rates.Add(new BookRateEntity { Id = rate.Id, User = user, Rate = rate.Rate });
                }
            }
        }

        public async Task DeleteAsync(Guid id, CancellationToken token)
        {
            var entity = await _ctx.Books.SingleAsync(x => x.Id == id, token);
            _ctx.Books.Remove(entity);
        }
    }
}