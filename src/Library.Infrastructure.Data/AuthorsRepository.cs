﻿using System;
using System.Collections.Generic;
using System.Linq;
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

        public void Create(Author author)
        {
            var entity = author.ToEntity();
            _ctx.Authors.Add(entity);
        }

        public Author GetById(Guid id)
        {
            return _ctx.Authors.Include(x => x.Books).ThenInclude(x => x.Book).Single(x => x.Id == id).ToAuthor(_entityFactory);
        }

        public void Delete(Guid id)
        {
            var author = _ctx.Authors.Single(x => x.Id == id);
            _ctx.Remove(author);
        }

        public void Update(Author author)
        {
            var entity = _ctx.Authors.Include(x => x.Books).ThenInclude(x => x.Book).Single(x => x.Id == author.Id);
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
                    entity.Books.Add(new BookAuthorEntity { Book = newBook.ToEntity()});
                }
            }
        }

        public IEnumerable<Author> GetByName(string firstName, string lastName)
        {
            return _ctx.Authors.Where(x => x.Name == firstName && x.SurName == lastName).Select(x => x.ToAuthor(_entityFactory, false)).ToList();
        }
    }
}