using System;
using Library.Application.Common;

namespace Library.Application.Commands.CreateBook
{
    public class CreateBookResult : RequestResult
    {
        public CreateBookResult(Guid bookId)
        : base(null)
        {
            BookId = bookId;
        }

        public CreateBookResult(Exception exception) : base(exception)
        {
        }

        public CreateBookResult(AggregateException exceptions) : base(exceptions)
        {
        }

        public Guid BookId { get; }
    }
}