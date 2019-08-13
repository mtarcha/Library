using MediatR;

namespace Library.Application.Queries.GetAuthors
{
    public class GetAuthorsQuery : IRequest<GetAuthorsResult>
    {
        public string SubName { get; set; }
    }
}