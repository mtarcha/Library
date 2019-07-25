using System;
using Microsoft.AspNetCore.Identity;

namespace Library.DataAccess.Entities
{
    internal sealed class UserEntity : IdentityUser
    {
        public DateTime DateOfBirth { get; set; }
    }
}