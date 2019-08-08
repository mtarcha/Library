using System;

namespace Library.Infrastucture.Data.Entities
{
    public class BookRateEntity
    {
        public Guid Id { get; set; }

        public UserEntity User { get; set; }

        public int Rate { get; set; }

        //public int BookId { get; set; }

        public BookEntity Book { get; set; }
    }
}