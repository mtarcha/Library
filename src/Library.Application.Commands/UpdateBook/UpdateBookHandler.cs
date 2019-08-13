using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Library.Application.Common;
using Library.Domain;
using MediatR;

namespace Library.Application.Commands.UpdateBook
{
    public class UpdateBookHandler : IRequestHandler<UpdateBookCommand, RequestResult>
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly IEntityFactory _entityFactory;

        public UpdateBookHandler(IUnitOfWorkFactory unitOfWorkFactory, IEntityFactory entityFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _entityFactory = entityFactory;
        }

        public async Task<RequestResult> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            using (var uow = _unitOfWorkFactory.Create())
            {
                try
                {
                    var bookModel = _entityFactory.CreateBook(
                        request.UpdatedBook.Id,
                        request.UpdatedBook.Name,
                        request.UpdatedBook.Date,
                        request.UpdatedBook.Summary,
                        request.UpdatedBook.Picture,
                        new List<BookRate>());

                    foreach (var author in request.UpdatedBook.Authors)
                    {
                        var authorModel = _entityFactory.CreateAuthor(
                            author.Id,
                            author.FirstName,
                            author.LastName,
                            new LifePeriod(author.DateOfBirth, author.DateOfDeath));

                        bookModel.AddAuthor(authorModel);
                    }

                    await uow.Books.UpdateAsync(bookModel, cancellationToken);

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