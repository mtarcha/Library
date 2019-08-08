using System;

namespace Library.Infrastucture.Data.Entities
{
    public class BookAuthorEntity
    {
        public Guid BookId { get; set; }
        public Guid AuthorId { get; set; }

        public BookEntity Book { get; set; }
        public AuthorEntity Author { get; set; }
    }
}