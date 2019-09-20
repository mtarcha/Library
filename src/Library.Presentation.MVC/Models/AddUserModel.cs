using System;

namespace Library.Presentation.MVC.Models
{
    public class AddUserModel
    {
        public string UserName { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime DateOfBirth { get; set; }
    }
}