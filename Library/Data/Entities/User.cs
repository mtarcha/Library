using System;
using Microsoft.AspNetCore.Identity;

namespace Library.Data.Entities
{
    public class User : IdentityUser
    {
        public DateTime DateOfBirth { get; set; }
    }
}