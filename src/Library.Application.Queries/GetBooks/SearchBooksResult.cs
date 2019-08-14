using System;
using System.Collections.Generic;
using Library.Application.Common;
using Library.Application.Queries.Common;

namespace Library.Application.Queries.GetBooks
{
    public class SearchBooksResult : RequestResult
    {
        public SearchBooksResult(IEnumerable<Book> books, int allBooksCount, int foundBooksCount)
        : base(null)
        {
            Books = books;
            AllBooksCount = allBooksCount;
            FoundBooksCount = foundBooksCount;
        }

        public SearchBooksResult(Exception exception) : base(exception)
        {
        }

        public SearchBooksResult(AggregateException exceptions) : base(exceptions)
        {
        }

        public int AllBooksCount { get; set; }

        public int FoundBooksCount { get; set; }

        public IEnumerable<Book> Books { get; set; }
    }
}