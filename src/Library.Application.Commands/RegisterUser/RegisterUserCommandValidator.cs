using System;
using FluentValidation;

namespace Library.Application.Commands.RegisterUser
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator()
        {
            RuleFor(x => x.UserName).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
            RuleFor(x => x.PhoneNumber).Matches("\\d+").NotEmpty();
            RuleFor(x => x.DateOfBirth).Must(x => x.Date < DateTime.Today.Date);
        }
    }
}