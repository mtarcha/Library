using System;
using System.Collections.Generic;
using System.Linq;
using Library.Domain.Common;
using Library.Domain.Events;

namespace Library.Domain.Entities
{
    // todo: separate aggregation roots, communication only through domain events
    public class User : Entity<Guid>, IAggregateRoot
    {
        private readonly List<Book> _favoriteBooks;
        private readonly List<Book> _recommendedToRead;
        private readonly List<User> _favoriteReviewers;

        internal User(IEventDispatcher eventDispatcher, string userName, DateTime dateOfBirth)
            : this(eventDispatcher, userName, dateOfBirth, null)
        {
        }

        internal User(IEventDispatcher eventDispatcher, string userName, DateTime dateOfBirth, Role role)
            : this(Guid.NewGuid(), eventDispatcher, userName, dateOfBirth, role, new List<Book>(), new List<Book>(), new List<User>())
        {
        }

        internal User(
            Guid id, 
            IEventDispatcher eventDispatcher, 
            string userName, 
            DateTime dateOfBirth, 
            Role role,
            IEnumerable<Book> favoriteBooks,
            IEnumerable<Book> recommendedBooks,
            IEnumerable<User> favoriteReviewers) 
            : base(id, eventDispatcher)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentNullException(nameof(userName));
            }

            if (dateOfBirth.Date > DateTime.Now.Date)
            {
                throw new ArgumentException(nameof(dateOfBirth));
            }

            UserName = userName;
            DateOfBirth = dateOfBirth;
            Role = role;

            _favoriteBooks = favoriteBooks.ToList();
            _recommendedToRead = recommendedBooks.ToList();
            _favoriteReviewers = favoriteReviewers.ToList();
        }

        public string UserName { get; }

        public DateTime DateOfBirth { get; }

        public Role Role { get; private set; }

        public string Token { get; set; }

        public string Password { get; private set; }

        public string NewPassword { get; private set; }

        public IReadOnlyList<Book> FavoriteBooks => _favoriteBooks;

        public IReadOnlyList<User> FavoriteReviewers => _favoriteReviewers;

        public IReadOnlyList<Book> RecommendedToRead => _recommendedToRead;

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
                RaiseEventDeferred(new FavoriteBookAddedEvent(Id, book.Id));
            }
        }

        public void AddRecommendedBook(Book book)
        {
            if (!_recommendedToRead.Exists(x => x.Id == book.Id))
            {
                _recommendedToRead.Add(book);
                RaiseEventDeferred(new RecommendedBookAddedEvent(Id, book.Id));
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