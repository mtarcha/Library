using System;

namespace Library.Data.Entities
{
    public class BookRateEntity
    {
        public int Id { get; set; }

        public Guid ReferenceId { get; set; }

        public UserEntity User { get; set; }

        public int Rate { get; set; }

        public BookEntity Book { get; set; }
    }
}