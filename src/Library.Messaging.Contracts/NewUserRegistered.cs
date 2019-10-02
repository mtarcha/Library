using System;

namespace Library.Messaging.Contracts
{
    public class NewUserRegistered
    {
        public string UserName { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime DateOfBirth { get; set; }
    }
}