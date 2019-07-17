using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Data;
using Library.Data.Entities;
using Library.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Author = Library.ViewModels.Author;

namespace Library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class BooksController : ControllerBase
    {
        private readonly ILibraryRepository _repository;
        private readonly ILogger<BooksController> _logger;

        public BooksController(ILibraryRepository repository, ILogger<BooksController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<IEnumerable<BookDescription>> Get()
        {
            try
            {
                return Ok(_repository.GetAllBooksIncludingAuthors().Select(book => new BookDescription(
                book.Name,
                book.Date,
                book.Authors.Select(author => new Author(author.Author.Name, author.Author.SurName)).ToArray(),
                book.Summary,
                10)));
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception: {e}");
                return BadRequest("failed to get boogs");
            }
           
        }
    }
}