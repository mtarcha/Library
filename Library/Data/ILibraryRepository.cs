using System.Collections.Generic;
using Library.Data.Entities;

namespace Library.Data
{
    public interface ILibraryRepository
    {
        IEnumerable<Book> GetAllBooks();
        IEnumerable<Book> GetBooksByName(string name);
        IEnumerable<Book> GetBooksByAuthorName(string name);

        IEnumerable<Author> GetAllAuthors();
        Author GetAuthor(int id);
        void AddNewAuthor(Author authorModel);
        void AddNewBook(Book bookModel);
    }
}