using System.Collections.Generic;
using Library.Application.Queries.Common;

namespace Library.Application.Queries.GetBooks
{
    public class SearchBooksResult
    {
        public SearchBooksResult(IEnumerable<Book> books, int totalBooksCount, int foundBooksCount)
        {
            Books = books;
            TotalBooksCount = totalBooksCount;
            FoundBooksCount = foundBooksCount;
        }

        public int TotalBooksCount { get; set; }

        public int FoundBooksCount { get; set; }

        public IEnumerable<Book> Books { get; set; }
    }
}