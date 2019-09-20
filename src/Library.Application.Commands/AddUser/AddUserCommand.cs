using System;
using Library.Application.Commands.Common;
using MediatR;

namespace Library.Application.Commands.RegisterUser
{
    public class AddUserCommand : IRequest<User>
    {
        public string UserName { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime DateOfBirth { get; set; }
    }
}