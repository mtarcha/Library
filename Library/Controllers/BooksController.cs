using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Library.Data;
using Library.Data.Entities;
using Library.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Library.Controllers
{
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
            return View(new BookViewModel());
        }

        [HttpPost]
        public IActionResult Create(BookViewModel bookViewModel)
        {

            if (ModelState.IsValid)
            {
                var bookModel = _mapper.Map<BookViewModel, Book>(bookViewModel);
                _repository.AddNewBook(bookModel);

                return RedirectToAction("Index", "Default");
            }

            return View(bookViewModel);
        }
    }
}