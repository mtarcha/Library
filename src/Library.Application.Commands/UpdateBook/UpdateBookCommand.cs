using Library.Application.Common;
using MediatR;

namespace Library.Application.Commands.UpdateBook
{
    public class UpdateBookCommand : IRequest<RequestResult>
    {
        public Book UpdatedBook { get; set; }
    }
}