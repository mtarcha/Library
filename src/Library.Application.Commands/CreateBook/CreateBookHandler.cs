using System.Threading;
using System.Threading.Tasks;
using Library.Application.Commands.Common;
using Library.Domain;
using MediatR;

namespace Library.Application.Commands.CreateBook
{
    public class CreateBookHandler : IRequestHandler<CreateBookCommand, Book>
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly IEntityFactory _entityFactory;

        public CreateBookHandler(IUnitOfWorkFactory unitOfWorkFactory, IEntityFactory entityFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _entityFactory = entityFactory;
        }

        public async Task<Book> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            using (var uow = _unitOfWorkFactory.Create())
            {
                var bookModel = _entityFactory.CreateBook(request.Name, request.Date, request.Summary, request.Picture);

                var result = await uow.Books.CreateAsync(bookModel, cancellationToken);

                return new Book(result);
            }
        }
    }
}