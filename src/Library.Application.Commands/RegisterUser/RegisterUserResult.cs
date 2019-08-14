using System;
using Library.Application.Common;

namespace Library.Application.Commands.RegisterUser
{
    public class RegisterUserResult : RequestResult
    {
        public RegisterUserResult(Guid userId)
            : base(null)
        {
            UserId = userId;
        }

        public RegisterUserResult(Exception exception) 
            : base(exception)
        {
        }

        public Guid UserId { get; }
    }
}