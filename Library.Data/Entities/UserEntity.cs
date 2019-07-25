using System;
using Microsoft.AspNetCore.Identity;

namespace Library.Data.Entities
{
    public sealed class UserEntity : IdentityUser
    {
        public DateTime DateOfBirth { get; set; }
    }
}