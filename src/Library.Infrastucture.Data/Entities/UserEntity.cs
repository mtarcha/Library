using System;
using Microsoft.AspNetCore.Identity;

namespace Library.Infrastucture.Data.Entities
{
    public sealed class UserEntity : IdentityUser
    {
        public Guid ReferenceId { get; set; }

        public DateTime DateOfBirth { get; set; }
    }
}