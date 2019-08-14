using System;
using System.Threading;
using System.Threading.Tasks;
using Library.Domain;
using MediatR;

namespace Library.Application.Commands.CreateBook
{
    public class CreateBookHandler : IRequestHandler<CreateBookCommand, CreateBookResult>
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly IEntityFactory _entityFactory;

        public CreateBookHandler(IUnitOfWorkFactory unitOfWorkFactory, IEntityFactory entityFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _entityFactory = entityFactory;
        }

        public async Task<CreateBookResult> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            using (var uow = _unitOfWorkFactory.Create())
            {
                try
                {
                    var bookModel = _entityFactory.CreateBook(request.Name, request.Date, request.Summary, request.Picture);

                    await uow.Books.CreateAsync(bookModel, cancellationToken);

                    return new CreateBookResult(bookModel.Id);
                }
                catch (Exception e)
                {
                    return new CreateBookResult(e);
                }
            }
        }
    }
}