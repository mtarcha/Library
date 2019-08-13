using Library.Application.Common;
using MediatR;

namespace Library.Application.Commands.CreateBook
{
    public class CreateBookCommand : IRequest<CreateBookResult>
    {
        public Book Book { get; set; }
    }
}