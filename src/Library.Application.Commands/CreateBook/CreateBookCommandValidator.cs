using System;
using FluentValidation;

namespace Library.Application.Commands.CreateBook
{
    public class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
    {
        public CreateBookCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Picture).Must(x => x.Length > 0);
            RuleFor(x => x.Date).Must(x => x.Date < DateTime.Today.Date.AddDays(1));
            RuleFor(x => x.Summary).NotEmpty();
        }
    }
}