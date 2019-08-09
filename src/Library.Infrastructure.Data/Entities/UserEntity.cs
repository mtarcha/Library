using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Library.Infrastructure.Data.Entities
{
    public sealed class UserEntity : IdentityUser<Guid>, IEntity
    {
        public DateTime DateOfBirth { get; set; }

        public List<UserEntity> FavoriteReviewers { get; set; }

        public List<BookEntity> FavoriteBooks { get; set; }

        public List<BookEntity> RecommendedToRead { get; set; }
    }
}