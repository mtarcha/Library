using FluentValidation;

namespace Library.Application.Queries.GetBooks
{
    public class GetBooksQueryValidator : AbstractValidator<GetBooksQuery>
    {
        public GetBooksQueryValidator()
        {
            RuleFor(x => x.SearchPattern).NotNull();
            RuleFor(x => x.SkipCount).Must(x => x >= 0);
            RuleFor(x => x.TakeCount).Must(x => x > 0);
        }
    }
}