using System;
using Microsoft.AspNetCore.Identity;

namespace Library.Presentation.MVC.Accounts
{
    public class UserAccount : IdentityUser
    {
        public DateTime DateOfBirth { get; set; }
    }
}