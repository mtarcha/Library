using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Library.Presentation.MVC.Clients;
using Library.Presentation.MVC.Models;
using Library.Presentation.MVC.Utility;
using Library.Presentation.MVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Book = Library.Presentation.MVC.Models.Book;

namespace Library.Presentation.MVC.Controllers
{
    public class BooksController : Controller
    {
        public const int BooksOnPage = 8;

        private readonly IBooksClient _booksClient;
        private readonly IMapper _mapper;

        public BooksController(IBooksClient booksClient, IMapper mapper)
        {
            _booksClient = booksClient;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Search([FromQuery(Name = "pattern")]string search = "", [FromQuery(Name = "page")]int page = 1)
        {
            if (page < 1)
            {
                return NotFound($"Invalid page '{page}'! Should be 1 or more.");
            }

            var searchPattern = search ?? string.Empty;
            var result = await _booksClient.Get( searchPattern, (page - 1) * BooksOnPage, BooksOnPage);

            if (result.ResponseMessage.StatusCode != HttpStatusCode.OK)
            {
                ModelState.AddModelError(string.Empty, result.ResponseMessage.Content.ToString());
            }
            else
            {
                var books = result.GetContent();
                var totalPages = (int)Math.Ceiling(books.TotalBooksCount / (decimal)BooksOnPage);

                var vm = new BooksViewModel
                {
                    TotalBooksCount = books.TotalBooksCount,
                    Pagination = new PaginationViewModel(totalPages, page),
                    BooksOnPage = _mapper.Map<IEnumerable<Book>, IEnumerable<BookViewModel>>(books.Books),
                    SearchPattern = searchPattern
                };

                return View(vm);
            }

            return View();
        }

        [HttpGet]
        [Authorize(Roles = Constants.UserRoleName + "," + Constants.AdminRoleName)]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = Constants.UserRoleName + "," + Constants.AdminRoleName)]
        public async Task<IActionResult> Create(CreateBookViewModel bookViewModel)
        {
            if (ModelState.IsValid)
            {
                var model = _mapper.Map<CreateBookViewModel, CreateBookModel>(bookViewModel);

                var result = await _booksClient.Create(model);

                if (result.ResponseMessage.StatusCode != HttpStatusCode.OK)
                {
                    ModelState.AddModelError(string.Empty, result.ResponseMessage.Content.ToString());
                }
                else
                {
                    return RedirectToAction("Search");
                }
            }

            return View(bookViewModel);
        }

        [HttpGet]
        [Authorize(Roles = Constants.AdminRoleName)]
        public async Task<IActionResult> Edit(Guid id)
        {
            var result = await _booksClient.GetBook(id);

            var book = result.GetContent();
            var editBook = _mapper.Map<Book, UpdateBookViewModel>(book);

            return View(editBook);
        }

        [HttpPost]
        [Authorize(Roles = Constants.AdminRoleName)]
        public async Task<IActionResult> Edit(UpdateBookViewModel bookViewModel)
        {
            if (ModelState.IsValid)
            {
                var model = _mapper.Map<UpdateBookViewModel, UpdateBookModel>(bookViewModel);
                var result = await _booksClient.UpdateBook(model);

                if (result.ResponseMessage.StatusCode != HttpStatusCode.OK)
                {
                    ModelState.AddModelError(string.Empty, result.ResponseMessage.Content.ToString());
                }
                else
                {
                    return RedirectToAction("Search");
                }
            }

            return View(bookViewModel);
        }

        [HttpPost]
        [Authorize(Roles = Constants.UserRoleName)]
        public async Task<IActionResult> SetRate(SetRateViewModel setRateViewModel)
        {
            var model = _mapper.Map<SetRateViewModel, SetRateModel>(setRateViewModel);
            await _booksClient.SetRate(model);

            return RedirectToAction("Search");
        }
    }
}