using System.Collections.Generic;
using Library.Data.Entities;

namespace Library.Data
{
    public interface ILibraryRepository
    {
        IEnumerable<Book> GetAllBooksIncludingAuthors();
    }
}