using System;
using MediatR;

namespace Library.Application.Queries.GetBook
{
    public class GetBookQuery : IRequest<GetBookResult>
    {
        public Guid BookId { get; set; }
    }
}