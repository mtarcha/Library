using System;
using Library.Application.Commands.Common;
using MediatR;

namespace Library.Application.Commands.CreateBook
{
    public class CreateBookCommand : IRequest<Book>
    {
        public string Name { get; set; }

        public byte[] Picture { get; set; }

        public DateTime Date { get; set; }

        public string Summary { get; set; }
    }
}