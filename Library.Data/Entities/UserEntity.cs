using System;
using Microsoft.AspNetCore.Identity;

namespace Library.Data.Entities
{
    public sealed class UserEntity : IdentityUser
    {
        public Guid ReferenceId { get; set; }
    }
}