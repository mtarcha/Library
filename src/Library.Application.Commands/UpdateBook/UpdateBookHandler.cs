using System;
using System.Threading;
using System.Threading.Tasks;
using Library.Application.Common;
using Library.Domain;
using Library.Domain.Entities;
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
                    var bookModel = _entityFactory.CreateBook(request.Id, request.Name, request.Date, request.Summary, request.Picture);

                    foreach (var author in request.Authors)
                    {
                        if (author.Id.HasValue && author.Id != Guid.Empty)
                        {
                            var authorModel = _entityFactory.CreateAuthor(
                                author.Id.Value,
                                author.FirstName,
                                author.LastName,
                                new LifePeriod(author.DateOfBirth, author.DateOfDeath));

                            bookModel.AddAuthor(authorModel);
                        }
                        else
                        {
                            var saved =  await uow.Authors.FindAsync(
                                author.FirstName, 
                                author.LastName, 
                                author.DateOfBirth, 
                                author.DateOfDeath, 
                                cancellationToken);

                            if (saved == null)
                            {
                                var authorModel = _entityFactory.CreateAuthor(
                                    author.FirstName,
                                    author.LastName,
                                    new LifePeriod(author.DateOfBirth, author.DateOfDeath));

                                bookModel.AddAuthor(authorModel);
                            }
                            else
                            {
                                bookModel.AddAuthor(saved);
                            }
                        }
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