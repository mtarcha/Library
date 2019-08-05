using System;
using System.Collections.Generic;
using Library.Domain.Common;

namespace Library.Domain
{
    public class User : Entity<Guid>, IAggregateRoot
    {
        private readonly List<Book> _favoriteBooks;
        private readonly List<User> _favoriteReviewers;

        internal User(EventDispatcher eventDispatcher, string userName, DateTime dateOfBirth)
            : this(eventDispatcher, userName, dateOfBirth, null)
        {
        }

        internal User(EventDispatcher eventDispatcher, string userName, DateTime dateOfBirth, Role role)
            : this(Guid.NewGuid(), eventDispatcher, userName, dateOfBirth, role)
        {
        }

        internal User(Guid id, EventDispatcher eventDispatcher, string userName, DateTime dateOfBirth, Role role) 
            : base(id, eventDispatcher)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentNullException(nameof(userName));
            }

            if (dateOfBirth > DateTime.Now)
            {
                throw new ArgumentException(nameof(dateOfBirth));
            }

            UserName = userName;
            DateOfBirth = dateOfBirth;
            Role = role;

            _favoriteBooks = new List<Book>();
            _favoriteReviewers = new List<User>();
        }

        public string UserName { get; }

        public DateTime DateOfBirth { get; }

        public Role Role { get; private set; }

        public string Password { get; private set; }

        public string NewPassword { get; private set; }

        public IReadOnlyList<Book> FavoriteBooks => _favoriteBooks;

        public IReadOnlyList<User> FavoriteReviewers => _favoriteReviewers;

        public void SetRole(Role role)
        {
            Role = role;
        }

        public void SetPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(nameof(password));
            }

            Password = password;
        }

        public void ChangePassword(string currentPassword, string newPassword)
        {
            if (string.IsNullOrEmpty(currentPassword))
            {
                throw new ArgumentNullException(nameof(currentPassword));
            }

            if (string.IsNullOrEmpty(newPassword))
            {
                throw new ArgumentNullException(nameof(newPassword));
            }

            Password = currentPassword;
            NewPassword = newPassword;
        }

        public void AddFavoriteBook(Book book)
        {
            if (!_favoriteBooks.Exists(x => x.Id == book.Id))
            {
                _favoriteBooks.Add(book);
            }
        }

        public void AddFavoriteReviewer(User reviewer)
        {
            if (!_favoriteReviewers.Exists(x => x.Id == reviewer.Id))
            {
                _favoriteReviewers.Add(reviewer);
            }
        }
    }
}