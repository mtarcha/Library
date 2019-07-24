using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Library.Data.Entities;

namespace Library.Data
{
    public interface ILibraryRepository
    {
        IEnumerable<Book> GetAllBooks();
        Book GetBook(int id);
        IEnumerable<Book> GetBooksByName(string name);
        IEnumerable<Book> GetBooksByAuthorName(string name);
        IQueryable<Book> Get(Expression<Func<Book, bool>> predicate);

        IEnumerable<Author> GetAllAuthors();
        Author GetAuthor(int id);
        Author GetAuthor(string name, string surName);
        void AddNewAuthor(Author authorModel);
        void AddNewBook(Book bookModel);
        void EditBook(Book bookModel);
    }
}