using System;
using System.Collections.Generic;
using System.IO;
using AutoMapper;
using Library.Business;
using Library.Domain;
using Library.Presentation.MVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Book = Library.Business.DTO.Book;

namespace Library.Presentation.MVC.Controllers
{
    public class BooksController : Controller
    {
        public const int BooksOnPage = 8;

        private readonly IBookService _bookService;
        private readonly IMapper _mapper;
        
        public BooksController(IBookService bookService, IMapper mapper)
        {
            _bookService = bookService;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get([FromQuery(Name = "pattern")]string search = "", [FromQuery(Name = "page")]int page = 1)
        {
            var pattern = search ?? string.Empty;
            var totalCount = _bookService.GetBooksCount(pattern);

            var totalPages = (int)Math.Ceiling(totalCount / (decimal)BooksOnPage);

            if (page < 1 || (page > totalPages && totalPages > 0))
            {
                return NotFound($"Invalid page '{page}'! Should be in range from 1 to {totalPages}");
            }

            var books = _bookService.GetBooks(pattern, (page - 1) * BooksOnPage, BooksOnPage);

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
        [Authorize(Roles = Role.UserRoleName + "," + Role.AdminRoleName)]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = Role.UserRoleName + "," + Role.AdminRoleName)]
        public IActionResult Create(CreateBookViewModel bookViewModel)
        {
            if (ModelState.IsValid)
            {
                var book = _mapper.Map<CreateBookViewModel, Book>(bookViewModel);

                _bookService.Create(book);

                return RedirectToAction("Get");
            }

            return View(bookViewModel);
        }

        [HttpGet]
        [Authorize(Roles = Role.AdminRoleName)]
        public IActionResult Edit(Guid id)
        {
            var book = _bookService.GetById(id);
            var editBook = _mapper.Map<Book, EditBookViewModel>(book);

            return View(editBook);
        }

        [HttpPost]
        [Authorize(Roles = Role.AdminRoleName)]
        public IActionResult Edit(EditBookViewModel bookViewModel)
        {
            if (ModelState.IsValid)
            {
                var book = _mapper.Map<EditBookViewModel, Book>(bookViewModel);
                _bookService.Update(book);

                return RedirectToAction("Get");
            }

            return View(bookViewModel);
        }

        [HttpPost]
        [Authorize]
        public IActionResult SetRate(SetRateViewModel setRateViewModel)
        {
            var userName = User.Identity.Name;

            _bookService.SetRate(userName, setRateViewModel.BookId, setRateViewModel.Rate);

            return Ok();
        }
    }
}