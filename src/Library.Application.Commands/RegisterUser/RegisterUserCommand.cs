using System;
using MediatR;

namespace Library.Application.Commands.RegisterUser
{
    public class RegisterUserCommand : IRequest<RegisterUserResult>
    {
        public string UserName { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Password { get; set; }
    }
}