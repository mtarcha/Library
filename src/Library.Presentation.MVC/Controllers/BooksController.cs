using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Library.Application.Commands.CreateBook;
using Library.Application.Commands.SetBookRate;
using Library.Application.Commands.UpdateBook;
using Library.Application.Queries.GetAuthors;
using Library.Application.Queries.GetBook;
using Library.Application.Queries.GetBooks;
using Library.Domain;
using Library.Presentation.MVC.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Book = Library.Application.Common.Book;

namespace Library.Presentation.MVC.Controllers
{
    public class BooksController : Controller
    {
        public const int BooksOnPage = 8;

        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public BooksController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get([FromQuery(Name = "pattern")]string search = "", [FromQuery(Name = "page")]int page = 1)
        {
            if (page < 1)
            {
                return NotFound($"Invalid page '{page}'! Should be 1 or more.");
            }

            var pattern = search ?? string.Empty;
            var query = new GetBooksQuery
            {
                SearchPattern = search,
                SkipCount = (page - 1) * BooksOnPage,
                TakeCount = BooksOnPage
            };

            var result = await _mediator.Send(query);

            if (result.HasErrors)
            {
                foreach (var exception in result.Exceptions.InnerExceptions)
                {
                    ModelState.AddModelError(String.Empty, exception.Message);
                }

                return BadRequest(ModelState);
            }
            else
            {
                var totalPages = (int)Math.Ceiling(result.AllBooksCount / (decimal)BooksOnPage);

                var vm = new BooksViewModel
                {
                    TotalBooksCount = result.AllBooksCount,
                    Pagination = new PaginationViewModel(totalPages, page),
                    BooksOnPage = _mapper.Map<IEnumerable<Book>, IEnumerable<BookViewModel>>(result.Books),
                    SearchPattern = pattern
                };

                return View(vm);
            }
        }

        [HttpGet]
        [Authorize(Roles = Role.UserRoleName + "," + Role.AdminRoleName)]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = Role.UserRoleName + "," + Role.AdminRoleName)]
        public async Task<IActionResult> Create(CreateBookViewModel bookViewModel)
        {
            if (ModelState.IsValid)
            {
                var command = new CreateBookCommand
                {
                    Book = _mapper.Map<CreateBookViewModel, Book>(bookViewModel)
                };

                foreach (var author in command.Book.Authors)
                {
                    var getAuthors = new GetAuthorsQuery {SubName = author.LastName};
                    var authors = await _mediator.Send(getAuthors);
                    var saved = authors.Authors.FirstOrDefault();

                    if (saved != null)
                    {
                        author.Id = saved.Id;
                    }
                }

                var result = _mediator.Send(command).Result;
                if (result.HasErrors)
                {
                    foreach (var error in result.Exceptions.InnerExceptions)
                    {
                        ModelState.AddModelError(string.Empty, error.Message);
                    }
                }
                else
                {
                    return RedirectToAction("Get");
                }
            }

            return View(bookViewModel);
        }

        [HttpGet]
        [Authorize(Roles = Role.AdminRoleName)]
        public async Task<IActionResult> Edit(Guid id)
        {
            var query = new GetBookQuery { BookId = id };

            var result = await _mediator.Send(query);

            var book = result.Book;
            var editBook = _mapper.Map<Book, EditBookViewModel>(book);

            return View(editBook);
        }

        [HttpPost]
        [Authorize(Roles = Role.AdminRoleName)]
        public IActionResult Edit(EditBookViewModel bookViewModel)
        {
            if (ModelState.IsValid)
            {
                var command = new UpdateBookCommand
                {
                    UpdatedBook = _mapper.Map<EditBookViewModel, Book>(bookViewModel)
                };

                var result = _mediator.Send(command).Result;
                if (result.HasErrors)
                {
                    foreach (var error in result.Exceptions.InnerExceptions)
                    {
                        ModelState.AddModelError(string.Empty, error.Message);
                    }
                }
                else
                {
                    return RedirectToAction("Get");
                }
            }

            return View(bookViewModel);
        }

        [HttpPost]
        [Authorize]
        public IActionResult SetRate(SetRateViewModel setRateViewModel)
        {
            var command = new SetBookRateCommand
            {
                UserName = User.Identity.Name,
                BookId = setRateViewModel.BookId,
                Rate = setRateViewModel.Rate
            };

            var result = _mediator.Send(command).Result;

            return RedirectToAction("Get");
        }
    }
}