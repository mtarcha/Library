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
                    var bookModel = _entityFactory
                        .CreateBook(request.Book.Name, request.Book.Date, request.Book.Summary, request.Book.Picture);

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