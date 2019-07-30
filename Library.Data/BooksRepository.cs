using System;
using System.Collections.Generic;
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

        public BooksRepository(LibraryContext ctx)
        {
            _ctx = ctx;

        }

        public void Create(Book book)
        {
            _ctx.Books.Add(book.ToEntity());

            _ctx.SaveChanges();
        }

        public Book GetById(Guid id)
        {
            return _ctx.Books.Where(x => x.ReferenceId == id).Include(x => x.Authors).ThenInclude(x => x.Author).Single().ToBook();
        }

        public void Delete(Guid id)
        {
            var entity = _ctx.Books.Single(x => x.ReferenceId == id);
            _ctx.Books.Remove(entity);

            _ctx.SaveChanges();
        }

        public void Update(Book book)
        {
            var entity = _ctx.Books.Where(x => x.ReferenceId == book.Id).Include(x => x.Authors).ThenInclude(x => x.Author).Single();
            entity.Name = book.Name;
            entity.Date = book.Date;
            entity.Summary = book.Summary;
            entity.Picture = book.Picture;

            if (book.Authors != null)
            {
                var savedAuthors = entity.Authors.ToList();
                var currentAuthors = book.Authors.ToList();

                var updatedAuthors = savedAuthors.Where(x => currentAuthors.Exists(b => b.Id == x.Author.ReferenceId)).ToList();
                var newAuthors = currentAuthors.Where(x => !savedAuthors.Exists(b => b.Author.ReferenceId == x.Id)).ToList();
                var deletedAuthors = savedAuthors.Except(updatedAuthors);

                foreach (var deletedAuthor in deletedAuthors)
                {
                    entity.Authors.Remove(deletedAuthor);
                }

                foreach (var updatedAuthor in updatedAuthors)
                {
                    var update = currentAuthors.Single(x => x.Id == updatedAuthor.Author.ReferenceId);
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

            _ctx.SaveChanges();
        }

        public IEnumerable<Book> Get(Predicate<Book> predicate, int skipCount, int takeCount)
        {
            return _ctx.Books.Where(x => predicate(x.ToBook(true))).Include(x => x.Authors).ThenInclude(x => x.Author).Select(x => x.ToBook(true));
        }

        public int GetCount(Predicate<Book> predicate)
        {
            return _ctx.Books.Count(x => predicate(x.ToBook(true)));
        }
    }
}