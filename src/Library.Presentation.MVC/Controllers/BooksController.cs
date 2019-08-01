using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using Library.Domain;
using Library.Presentation.MVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Library.Presentation.MVC.Controllers
{
    public class BooksController : Controller
    {
        public const int BooksOnPage = 8;

        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly IMapper _mapper;
        private readonly ILogger<BooksController> _logger;


        public BooksController(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper, ILogger<BooksController> logger)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get([FromQuery(Name = "pattern")]string search = "", [FromQuery(Name = "page")]int page = 1)
        {
            using (var uow = _unitOfWorkFactory.Create())
            {
                var pattern = search ?? string.Empty;
                var totalCount = uow.Books.GetCount(x => x.Name.Contains(pattern, StringComparison.OrdinalIgnoreCase));

                var totalPages = (int)Math.Ceiling(totalCount / (decimal)BooksOnPage);

                if (page < 1 || (page > totalPages && totalPages > 0))
                {
                    return NotFound($"Invalid page '{page}'! Should be in range from 1 to {totalPages}");
                }

                var books = uow.Books.Get(x => x.Name.Contains(pattern, StringComparison.OrdinalIgnoreCase), (page - 1) * BooksOnPage, BooksOnPage).ToList();

                var vm = new BooksViewModel
                {
                    TotalBooksCount = totalCount,
                    Pagination = new PaginationViewModel(totalPages, totalPages, page),
                    BooksOnPage = _mapper.Map<IEnumerable<Book>, IEnumerable<BookViewModel>>(books),
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
        public IActionResult Create(CreateBookViewModel bookViewModel)
        {
            if (ModelState.IsValid)
            {
                using (var uow = _unitOfWorkFactory.Create())
                {
                    var imageData = ReadImageData(bookViewModel.Avatar);
                    var bookModel = new Book(bookViewModel.Name, bookViewModel.Date, bookViewModel.Summary, imageData);

                    foreach (var author in bookViewModel.Authors)
                    {
                        var saved = uow.Authors.GetByName(author.FirstName, author.LastName).FirstOrDefault();

                        if (saved != null)
                        {
                            bookModel.AddAuthor(saved);
                        }
                        else
                        {
                            bookModel.AddAuthor(new Author(author.FirstName, author.LastName,
                                new LifePeriod(author.DateOfBirth, author.DateOfDeath)));
                        }
                    }

                    uow.Books.Create(bookModel);

                    return RedirectToAction("Get");
                }
            }

            return View(bookViewModel);
        }

        [HttpGet]
        [Authorize(Roles = Role.AdminRoleName)]
        public IActionResult Edit(Guid id)
        {
            using (var uow = _unitOfWorkFactory.Create())
            {
                var book = uow.Books.GetById(id);
                var editBook = _mapper.Map<Book, EditBookViewModel>(book);

                return View(editBook);
            }
        }

        [HttpPost]
        [Authorize(Roles = Role.AdminRoleName)]
        public IActionResult Edit(EditBookViewModel bookViewModel)
        {
            if (ModelState.IsValid)
            {
                using (var uow = _unitOfWorkFactory.Create())
                {
                    var imageData = bookViewModel.Avatar != null ? ReadImageData(bookViewModel.Avatar) : bookViewModel.Picture;

                    var bookModel = new Book(bookViewModel.Id, bookViewModel.Name, bookViewModel.Date, bookViewModel.Summary, imageData);

                    foreach (var author in bookViewModel.Authors)
                    {
                        var authorModel = new Author(author.Id, author.FirstName, author.LastName, new LifePeriod(author.DateOfBirth, author.DateOfDeath));
                        bookModel.AddAuthor(authorModel);
                    }

                    uow.Books.Update(bookModel);

                    return RedirectToAction("Get");
                }
            }

            return View(bookViewModel);
        }

        [HttpPost]
        [Authorize]
        public IActionResult SetRate(SetRateViewModel setRateViewModel)
        {
            using (var uow = _unitOfWorkFactory.Create())
            {
                var userName = User.Identity.Name;
                var user = uow.Users.GetByName(userName);

                var book = uow.Books.GetById(setRateViewModel.BookId);

                book.SetRate(user, setRateViewModel.Rate);

                uow.Books.Update(book);

                return RedirectToAction("Get");
            }
        }

        private byte[] ReadImageData(IFormFile formFile)
        {
            byte[] imageData = null;
            using (var binaryReader = new BinaryReader(formFile.OpenReadStream()))
            {
                imageData = binaryReader.ReadBytes((int)formFile.Length);
            }

            return imageData;
        }
    }
}