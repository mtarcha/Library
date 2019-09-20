using System;
using System.Collections.Generic;

namespace Library.Presentation.MVC.Models
{
    public class User
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public IEnumerable<Author> FavoriteAuthors { get; set; }

        public IEnumerable<Book> FavoriteBooks { get; set; }

        public IEnumerable<Book> Recommended { get; set; }
    }
}