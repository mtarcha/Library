using System;
using System.Collections.Generic;

namespace Library.Domain
{
    public class User
    {
        public Guid Id { get; set; }

        public string Login { get; set; }

        public DateTime DateOfBirth { get; set; }

        public IEnumerable<Book> FavoriteBooks { get; set; }

        public IEnumerable<User> FavoriteReviewers { get; set; }
    }
}