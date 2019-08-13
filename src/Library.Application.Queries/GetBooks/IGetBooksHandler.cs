using MediatR;

namespace Library.Application.Queries.GetBooks
{
    public interface IGetBooksHandler : IRequestHandler<GetBooksQuery, SearchBooksResult>
    {
    }
}