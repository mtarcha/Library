using System;
using System.Threading;
using System.Threading.Tasks;
using Library.Application.Common;
using Library.Domain;
using MediatR;

namespace Library.Application.Commands.SetBookRate
{
    public class SetBookRateHandler : IRequestHandler<SetBookRateCommand, RequestResult>
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly IEntityFactory _entityFactory;

        public SetBookRateHandler(IUnitOfWorkFactory unitOfWorkFactory, IEntityFactory entityFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _entityFactory = entityFactory;
        }

        public async Task<RequestResult> Handle(SetBookRateCommand request, CancellationToken cancellationToken)
        {
            using (var uow = _unitOfWorkFactory.Create())
            {
                try
                {
                    var user = await uow.Users.GetByNameAsync(request.UserName, cancellationToken);

                    var book = await uow.Books.GetByIdAsync(request.BookId, cancellationToken);

                    book.SetRate(_entityFactory.CreateBookRate(user, request.Rate));

                    await uow.Books.UpdateAsync(book, cancellationToken);

                    return RequestResult.Success;
                }
                catch (Exception e)
                {
                    return new RequestResult(e);
                }
            } 
        }
    }
}