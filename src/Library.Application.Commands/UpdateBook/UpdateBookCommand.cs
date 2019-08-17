using System;
using System.Collections.Generic;
using Library.Application.Commands.Common;
using MediatR;

namespace Library.Application.Commands.UpdateBook
{
    public class UpdateBookCommand : IRequest<Book>
    {
        public UpdateBookCommand()
        {
            Authors = new List<UpdateAuthor>();
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public byte[] Picture { get; set; }

        public DateTime Date { get; set; }

        public ICollection<UpdateAuthor> Authors { get; set; }

        public string Summary { get; set; }
    }

    public class UpdateAuthor
    {
        public Guid? Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public DateTime? DateOfDeath { get; set; }
    }
}