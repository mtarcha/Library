using System;
using System.Collections.Generic;
using Library.Application.Common;
using Library.Application.Queries.Common;

namespace Library.Application.Queries.GetAuthors
{
    public class GetAuthorsResult : RequestResult
    {
        public GetAuthorsResult(IEnumerable<Author> authors)
        : base(null)
        {
            Authors = authors;
        }

        public GetAuthorsResult(Exception exception) : base(exception)
        {
        }

        public GetAuthorsResult(AggregateException exceptions) : base(exceptions)
        {
        }

        public IEnumerable<Author> Authors { get; }
    }
}