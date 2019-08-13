using MediatR;

namespace Library.Application.Queries.GetAuthors
{
    public interface IGetAuthorsQueryHandler : IRequestHandler<GetAuthorsQuery, GetAuthorsResult>
    { 
    }
}