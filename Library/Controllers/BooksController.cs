using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using Library.Domain;
using Library.Presentation.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Library.Presentation.Controllers
{
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
        [AllowAnonymous]
        public IActionResult Get([FromQuery(Name = "pattern")]string search = "", [FromQuery(Name = "page")]int page = 1)
        {
            var pattern = search ?? string.Empty;
            var totalCount = _booksRepository.GetCount(x => x.Name.Contains(pattern, StringComparison.OrdinalIgnoreCase));

            var totalPages = (int)Math.Ceiling(totalCount / (decimal)BooksOnPage);

            if (page < 1 || (page > totalPages && totalPages > 0))
            {
                return NotFound($"Invalid page '{page}'! Should be in range from 1 to {totalPages}");
            }

            var books = _booksRepository.Get(x => x.Name.Contains(pattern, StringComparison.OrdinalIgnoreCase), (page - 1) * BooksOnPage, BooksOnPage).ToList();

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
        [Authorize(Roles = Roles.User + "," + Roles.Admin)]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = Roles.User + "," + Roles.Admin)]
        public IActionResult Create(CreateBookViewModel bookViewModel)
        {
            if (ModelState.IsValid)
            {
                var imageData = ReadImageData(bookViewModel.Avatar);
                var bookModel = new Book(bookViewModel.Name, bookViewModel.Date, bookViewModel.Summary, imageData);

                foreach (var author in bookViewModel.Authors)
                {
                    var saved = _authorsRepository.GetByName(author.FirstName, author.LastName).FirstOrDefault();
                    
                    if (saved != null)
                    {
                        bookModel.AddAuthor(saved);
                    }
                    else
                    {
                        bookModel.AddAuthor(new Author(author.FirstName, author.LastName, new LifePeriod(author.DateOfBirth, author.DateOfDeath)));
                    }
                }

                _booksRepository.Create(bookModel);

                return RedirectToAction("Get");
            }

            return View(bookViewModel);
        }

        [HttpGet]
        [Authorize(Roles = Roles.Admin)]
        public IActionResult Edit(Guid id)
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
                var imageData =  bookViewModel.Avatar != null ? ReadImageData(bookViewModel.Avatar) : bookViewModel.Picture;

                var bookModel = new Book(bookViewModel.Id, bookViewModel.Name, bookViewModel.Date, bookViewModel.Summary, imageData);

                foreach (var author in bookViewModel.Authors)
                {
                    var authorModel = new Author(author.Id, author.FirstName, author.LastName, new LifePeriod(author.DateOfBirth, author.DateOfDeath));
                    bookModel.AddAuthor(authorModel);
                }
               
                _booksRepository.Update(bookModel);

                return RedirectToAction("Get");
            }

            return View(bookViewModel);
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