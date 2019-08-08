using System;
using System.Collections.Generic;
using System.Linq;
using Library.Domain;
using Library.Infrastucture.Data.Entities;
using Library.Infrastucture.Data.Internal;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastucture.Data
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

        public void Create(Book book)
        {
            var entity = book.ToEntity();
            _ctx.Books.Add(entity);
        }

        public Book GetById(Guid id)
        {
            return _ctx.Books.Where(x => x.Id == id)
                .Include(x => x.Authors).ThenInclude(x => x.Author)
                .Include(x => x.Rates).ThenInclude(x => x.User)
                .Single().ToBook(_entityFactory);
        }

        public void Delete(Guid id)
        {
            var entity = _ctx.Books.Single(x => x.Id == id);
            _ctx.Books.Remove(entity);
        }

        public void Update(Book book)
        {
            var entity = _ctx.Books.Where(x => x.Id == book.Id)
                .Include(x => x.Authors).ThenInclude(x => x.Author)
                .Include(x => x.Rates).Single();
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

        public IEnumerable<Book> Get(string searchPatter, int skipCount, int takeCount)
        {
            // todo: check if Contains will split sql query
            var books = _ctx.Books
                .Where(x => x.Name.Contains(searchPatter))
                .Include(x => x.Authors).ThenInclude(x => x.Author)
                .Include(x => x.Rates).ThenInclude(x => x.User)
                .OrderByDescending(x => x.Rate)
                .Skip(skipCount).Take(takeCount)
                .ToList();

            return books.Select(x => x.ToBook(_entityFactory));
        }

        public int GetCount(string searchPatter)
        {
            return _ctx.Books.Count(x => x.Name.Contains(searchPatter, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}