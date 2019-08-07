using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Library.Infrastucture.Data.Entities
{
    public sealed class UserEntity : IdentityUser
    {
        public Guid ReferenceId { get; set; }

        public DateTime DateOfBirth { get; set; }

        public List<UserEntity> FavoriteReviewers { get; set; }

        public List<BookEntity> FavoriteBooks { get; set; }

        public List<BookEntity> RecommendedToRead { get; set; }
    }
}