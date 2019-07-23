using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Library.Data;
using Library.Data.Entities;
using Library.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Library.Controllers
{
    [Authorize(Roles = Roles.User + "," + Roles.Admin)]
    public class BooksController : Controller
    {
        private readonly ILibraryRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<BooksController> _logger;

        public BooksController(ILibraryRepository repository, IMapper mapper, ILogger<BooksController> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<IEnumerable<Book>> Get()
        {
            try
            {
                return Ok(_repository.GetAllBooks());
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception: {e}");
                return BadRequest("failed to get boogs");
            }
        }

        [HttpGet("{name}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<IEnumerable<BookViewModel>> Get(string name)
        {
            try
            {
                return Ok(_mapper.Map<IEnumerable<Book>, IEnumerable<BookViewModel>>(_repository.GetBooksByName(name)));
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception: {e}");
                return BadRequest("failed to get boogs");
            }
        }

        [HttpGet("author/{name}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<IEnumerable<Book>> GetByAuthor(string name)
        {
            try
            {
                return Ok(_mapper.Map<IEnumerable<Book>, IEnumerable<BookViewModel>>(_repository.GetBooksByAuthorName(name)));
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception: {e}");
                return BadRequest("failed to get boogs");
            }
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
                var bookModel = _mapper.Map<CreateBookViewModel, Book>(bookViewModel);

                foreach (var author in bookModel.Authors)
                {
                    var saved = _repository.GetAuthor(author.Author.Name, author.Author.SurName);
                    if (saved != null)
                    {
                        author.Author = saved;
                    }
                }

                _repository.AddNewBook(bookModel);

                return RedirectToAction("Index", "Default");
            }

            return View(bookViewModel);
        }

        [HttpGet]
        [Authorize(Roles = Roles.Admin)]
        public IActionResult Edit(int id)
        {
            var book = _repository.GetBook(id);
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
                foreach (var author in bookModel.Authors)
                {
                    var saved = _repository.GetAuthor(author.Author.Name, author.Author.SurName);
                    if (saved != null)
                    {
                        author.Author = saved;
                    }
                }

                if (bookModel.Avatar?.Length == 0 && bookViewModel.Picture?.Length > 0)
                {
                    bookModel.Avatar = bookViewModel.Picture;
                }

                _repository.EditBook(bookModel);

                return RedirectToAction("Index", "Default");
            }

            return View(bookViewModel);
        }
    }
}