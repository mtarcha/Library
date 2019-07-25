using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Library.Domain;
using Library.Presentation.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Library.Presentation.Controllers
{
    [Authorize(Roles = Roles.User + "," + Roles.Admin)]
    public class BooksController : Controller
    {
        public const int BooksOnPage = 8;

        private readonly IMapper _mapper;
        private readonly ILogger<BooksController> _logger;
        private readonly IBooksRepository _booksRepository;
        private readonly IAuthorsRepository _authorsRepository;

        public BooksController(IBooksRepository booksRepository, IAuthorsRepository authorsRepository, IMapper mapper, ILogger<BooksController> logger)
        {
            _booksRepository = booksRepository;
            _authorsRepository = authorsRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get([FromQuery(Name = "pattern")]string search = "", [FromQuery(Name = "page")]int page = 1)
        {
            var pattern = search ?? string.Empty;
            var totalCount = _booksRepository.Get(x => x.Name.Contains(pattern)).Count();

            var totalPages = (int)Math.Ceiling(totalCount / (decimal)BooksOnPage);

            if (page < 1 || (page > totalPages && totalPages > 0))
            {
                return NotFound($"Invalid page '{page}'! Should be in range from 1 to {totalPages}");
            }

            var books = _booksRepository.Get(x => x.Name.Contains(pattern)).Skip((page - 1) * BooksOnPage).Take(BooksOnPage).ToList();

            var vm = new BooksViewModel
            {
                TotalBooksCount = totalCount,
                Pagination = new PaginationViewModel(totalPages, totalPages, page),
                BooksOnPage = _mapper.Map<IEnumerable<Book>, IEnumerable<BookViewModel>>(books),
                SearchPattern = pattern
            };

            return View(vm);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateBookViewModel bookViewModel)
        {
            if (ModelState.IsValid)
            {
                // todo: move to repository??
                foreach (var author in bookViewModel.Authors)
                {
                    var saved = _authorsRepository.Get(x => x.Name == author.FirstName && x.SurName == author.LastName).FirstOrDefault();
                    if (saved != null)
                    {
                        author.Id = saved.Id;
                    }
                }

                var bookModel = _mapper.Map<CreateBookViewModel, Book>(bookViewModel);

                _booksRepository.Create(bookModel);

                return RedirectToAction("Get");
            }

            return View(bookViewModel);
        }

        [HttpGet]
        [Authorize(Roles = Roles.Admin)]
        public IActionResult Edit(int id)
        {
            var book = _booksRepository.GetById(id);
            var editBook = _mapper.Map<Book, EditBookViewModel>(book);

            return View(editBook);
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        public IActionResult Edit(EditBookViewModel bookViewModel)
        {
            if (ModelState.IsValid)
            {
                var bookModel = _mapper.Map<EditBookViewModel, Book>(bookViewModel);
               
                _booksRepository.Update(bookModel);

                foreach (var authorViewModel in bookViewModel.Authors)
                {
                    var authorModel = _mapper.Map<AuthorViewModel, Author>(authorViewModel);
                    _authorsRepository.Update(authorModel);
                }

                return RedirectToAction("Get");
            }

            return View(bookViewModel);
        }
    }
}