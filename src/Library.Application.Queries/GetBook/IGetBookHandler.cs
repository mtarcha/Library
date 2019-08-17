using Library.Application.Queries.Common;
using MediatR;

namespace Library.Application.Queries.GetBook
{
    public interface IGetBookHandler : IRequestHandler<GetBookQuery, Book>
    {
    }
}