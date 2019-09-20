using System;
using FluentValidation;

namespace Library.Application.Commands.UpdateBook
{
    public class UpdateBookCommandValidator : AbstractValidator<UpdateBookCommand>
    {
        public UpdateBookCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Date).Must(x => x.Date < DateTime.Today.Date.AddDays(1));
            RuleForEach(x => x.Authors).Must(x => x.DateOfBirth.Date < DateTime.Today.Date
                                                  && x.FirstName != null
                                                  && x.LastName != null);
            RuleFor(x => x.Summary).NotEmpty();
        }
    }
}