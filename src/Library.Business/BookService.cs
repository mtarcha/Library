using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Library.Domain;
using Book = Library.Business.DTO.Book;

namespace Library.Business
{
    public class BookService : IBookService
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly IEntityFactory _entityFactory;
        private readonly IMapper _mapper;

        public BookService(IUnitOfWorkFactory unitOfWorkFactory, IEntityFactory entityFactory, IMapper mapper)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _entityFactory = entityFactory;
            _mapper = mapper;
        }

        public Book GetById(Guid id)
        {
            using (var uow = _unitOfWorkFactory.Create())
            {
                return _mapper.Map<Domain.Book, Book>(uow.Books.GetById(id));
            }
        }

        public int GetBooksCount(string searchPattern)
        {
            using (var uow = _unitOfWorkFactory.Create())
            {
                return uow.Books.GetCount(searchPattern);
            }
        }

        public IReadOnlyList<Book> GetBooks(string searchPattern, int skip, int take)
        {
            using (var uow = _unitOfWorkFactory.Create())
            {
                var domainBooks = uow.Books.Get(searchPattern, skip, take);

                return _mapper.Map<IEnumerable<Domain.Book>, IEnumerable<DTO.Book>>(domainBooks).ToList();
            }
        }

        public void Create(Book book)
        {
            using (var uow = _unitOfWorkFactory.Create())
            {
                var bookModel = _entityFactory.CreateBook(book.Name, book.Date, book.Summary, book.Picture);

                foreach (var author in book.Authors)
                {
                    var saved = uow.Authors.GetByName(author.FirstName, author.LastName).FirstOrDefault();

                    if (saved != null)
                    {
                        bookModel.AddAuthor(saved);
                    }
                    else
                    {
                        bookModel.AddAuthor(_entityFactory.CreateAuthor(author.FirstName, author.LastName,
                            new LifePeriod(author.DateOfBirth, author.DateOfDeath)));
                    }
                }

                uow.Books.Create(bookModel);
            }
        }

        public void Update(Book book)
        {
            using (var uow = _unitOfWorkFactory.Create())
            {
                var bookModel = _entityFactory.CreateBook(book.Id, book.Name, book.Date, book.Summary, book.Picture, new List<BookRate>());

                foreach (var author in book.Authors)
                {
                    var authorModel = _entityFactory.CreateAuthor(
                        author.Id, 
                        author.FirstName, 
                        author.LastName, 
                        new LifePeriod(author.DateOfBirth, author.DateOfDeath));

                    bookModel.AddAuthor(authorModel);
                }

                uow.Books.Update(bookModel);
            }
        }

        public void SetRate(string userName, Guid bookId, int rate)
        {
            using (var uow = _unitOfWorkFactory.Create())
            {
                var user = uow.Users.GetByName(userName);

                var book = uow.Books.GetById(bookId);

                book.SetRate(_entityFactory.CreateBookRate(user, rate));

                uow.Books.Update(book);
            }
        }
    }
}