using MediatR;

namespace Library.Application.Queries.GetBooks
{
    public class GetBooksQuery : IRequest<SearchBooksResult>
    {
        public string SearchPattern { get; set; }

        public int SkipCount { get; set; }

        public int TakeCount { get; set; }
    }
}