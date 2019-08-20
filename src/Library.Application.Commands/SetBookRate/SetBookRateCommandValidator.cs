using FluentValidation;

namespace Library.Application.Commands.SetBookRate
{
    public class SetBookRateCommandValidator : AbstractValidator<SetBookRateCommand>
    {
        public SetBookRateCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.BookId).NotEmpty();
            RuleFor(x => x.Rate).Must(x => x >= 1 && x <= 5);
        }
    }
}