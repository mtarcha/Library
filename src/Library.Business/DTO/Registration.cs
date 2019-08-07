using System;

namespace Library.Business.DTO
{
    public class Registration
    {
        public string UserName { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Password { get; set; }
    }
}