using FluentValidation;

namespace Library.Application.Queries.GetBook
{
    public class GetBookQueryValidator : AbstractValidator<GetBookQuery>
    {
        public GetBookQueryValidator()
        {
            RuleFor(x => x.BookId).NotEmpty();
        }
    }
}