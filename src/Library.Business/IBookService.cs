using System;
using System.Collections.Generic;
using Library.Business.DTO;

namespace Library.Business
{
    public interface IBookService
    {
        Book GetById(Guid id);

        int GetBooksCount(string searchPattern);

        IReadOnlyList<Book> GetBooks(string searchPattern, int skip, int take);

        void Create(Book book);

        void Update(Book book);

        void SetRate(string userName, Guid bookId, int rate);
    }
}