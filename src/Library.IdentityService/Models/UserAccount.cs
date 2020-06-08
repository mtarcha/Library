using System;
using Microsoft.AspNetCore.Identity;

namespace Library.IdentityService.Models
{
    public class UserAccount : IdentityUser
    {
        public DateTime DateOfBirth { get; set; }
    }
}