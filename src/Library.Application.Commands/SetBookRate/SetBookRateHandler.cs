using System.Threading;
using System.Threading.Tasks;
using Library.Domain;
using MediatR;

namespace Library.Application.Commands.SetBookRate
{
    public class SetBookRateHandler : IRequestHandler<SetBookRateCommand, SetBookRateResult>
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly IEntityFactory _entityFactory;

        public SetBookRateHandler(IUnitOfWorkFactory unitOfWorkFactory, IEntityFactory entityFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _entityFactory = entityFactory;
        }

        public async Task<SetBookRateResult> Handle(SetBookRateCommand request, CancellationToken cancellationToken)
        {
            using (var uow = _unitOfWorkFactory.Create())
            {
                var user = await uow.Users.GetByIdAsync(request.UserId, cancellationToken);

                var book = await uow.Books.GetByIdAsync(request.BookId, cancellationToken);

                book.SetRate(_entityFactory.CreateBookRate(user, request.Rate));

                await uow.Books.UpdateAsync(book, cancellationToken);

                return new SetBookRateResult {Id = book.Id, Rate = book.Rate.Value };
            } 
        }
    }
}