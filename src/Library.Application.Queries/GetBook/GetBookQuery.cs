using System;
using Library.Application.Queries.Common;
using MediatR;

namespace Library.Application.Queries.GetBook
{
    public class GetBookQuery : IRequest<Book>
    {
        public Guid BookId { get; set; }
    }
}