using System;
using System.Collections.Generic;

namespace Library.Infrastructure.Data.Entities
{
    public sealed class UserEntity : IEntity
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public List<UserEntity> FavoriteReviewers { get; set; }

        public List<BookEntity> FavoriteBooks { get; set; }

        public List<BookEntity> RecommendedToRead { get; set; }
    }
}