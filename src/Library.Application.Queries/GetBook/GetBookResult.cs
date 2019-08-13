using System;
using Library.Application.Common;

namespace Library.Application.Queries.GetBook
{
    public class GetBookResult : RequestResult
    {
        public GetBookResult(Book book)
        : base(null)
        {
            Book = book;
        }

        public GetBookResult(Exception exception) : base(exception)
        {
        }

        public GetBookResult(AggregateException exceptions) : base(exceptions)
        {
        }

        public Book Book { get; }
    }
}