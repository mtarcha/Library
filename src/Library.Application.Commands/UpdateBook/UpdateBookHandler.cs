using System;
using System.Threading;
using System.Threading.Tasks;
using Library.Domain;
using Library.Domain.Entities;
using MediatR;
using Book = Library.Application.Commands.Common.Book;

namespace Library.Application.Commands.UpdateBook
{
    public class UpdateBookHandler : IRequestHandler<UpdateBookCommand, Book>
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly IEntityFactory _entityFactory;

        public UpdateBookHandler(IUnitOfWorkFactory unitOfWorkFactory, IEntityFactory entityFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _entityFactory = entityFactory;
        }

        public async Task<Book> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            using (var uow = _unitOfWorkFactory.Create())
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
                        var saved = await uow.Authors.FindAsync(
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

                var result = await uow.Books.UpdateAsync(bookModel, cancellationToken);

                return new Book(result);
            }
        }
    }
}