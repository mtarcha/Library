using System;

namespace Library.Application.Commands.Common
{
    public class Author
    {
        internal Author(Domain.Entities.Author author)
        {
            Id = author.Id;
            FirstName = author.Name;
            LastName = author.SurName;
            DateOfBirth = author.LifePeriod.DateOfBirth;
            DateOfDeath = author.LifePeriod.DateOfDeath;
        }

        public Guid Id { get; }

        public string FirstName { get; }

        public string LastName { get; }

        public DateTime DateOfBirth { get;  }

        public DateTime? DateOfDeath { get;  }
    }
}