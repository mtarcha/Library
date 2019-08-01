using System;
using System.Collections.Generic;
using System.Linq;
using Library.Domain;
using Library.Infrastucture.Data.Entities;
using Library.Infrastucture.Data.Internal;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastucture.Data
{
    public sealed class AuthorsRepository : IAuthorsRepository
    {
        private readonly LibraryContext _ctx;
        
        public AuthorsRepository(LibraryContext ctx)
        {
            _ctx = ctx;
        }

        public void Create(Author author)
        {
            _ctx.Authors.Add(author.ToEntity());
        }

        public Author GetById(Guid id)
        {
            return _ctx.Authors.Include(x => x.Books).ThenInclude(x => x.Book).Single(x => x.ReferenceId == id).ToAuthor();
        }

        public void Delete(Guid id)
        {
            var author = _ctx.Authors.Single(x => x.ReferenceId == id);
            _ctx.Remove(author);
        }

        public void Update(Author author)
        {
            var entity = _ctx.Authors.Include(x => x.Books).ThenInclude(x => x.Book).Single(x => x.ReferenceId == author.Id);
            entity.Name = author.Name;
            entity.SurName = author.SurName;
            entity.DateOfBirth = author.LifePeriod.DateOfBirth;
            entity.DateOfDeath = author.LifePeriod.DateOfDeath;
            
            if (author.Books != null)
            {
                var savedBooks = entity.Books.ToList();
                var currentBooks = author.Books.ToList();

                var updatedBooks = savedBooks.Where(x => currentBooks.Exists(b => b.Id == x.Book.ReferenceId)).ToList();
                var newBooks = currentBooks.Where(x => !savedBooks.Exists(b => b.Book.ReferenceId == x.Id)).ToList();
                var deletedBooks = savedBooks.Except(updatedBooks);

                foreach (var deletedBook in deletedBooks)
                {
                    entity.Books.Remove(deletedBook);
                }

                foreach (var updatedBook in updatedBooks)
                {
                    var update = currentBooks.Single(x => x.Id == updatedBook.Book.ReferenceId);
                    updatedBook.Book.Name = update.Name;
                    updatedBook.Book.Date = update.Date;
                    updatedBook.Book.Picture = update.Picture;
                }

                foreach (var newBook in newBooks)
                {
                    entity.Books.Add(new BookAuthorEntity { Book = newBook.ToEntity()});
                }
            }
        }

        public IEnumerable<Author> GetByName(string firstName, string lastName)
        {
            return _ctx.Authors.Where(x => x.Name == firstName && x.SurName == lastName).Select(x => x.ToAuthor(false)).ToList();
        }
    }
}