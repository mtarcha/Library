using FluentValidation;

namespace Library.Application.Queries.GetAuthors
{
    public class GetAuthorsQueryValidator : AbstractValidator<GetAuthorsQuery>
    {
        public GetAuthorsQueryValidator()
        {
            RuleFor(x => x.SubName).NotEmpty();
        }
    }
}