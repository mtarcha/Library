using System;
using System.Collections.Generic;

namespace Library.Data.Entities
{
    public sealed class AuthorEntity : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string SurName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public DateTime? DateOfDeath { get; set; }

        public IEnumerable<BookAuthorEntity> Books { get; set; }
    }
}