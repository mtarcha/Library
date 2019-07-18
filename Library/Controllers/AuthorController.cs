using System;
using System.Collections.Generic;
using AutoMapper;
using Library.Data;
using Library.Data.Entities;
using Library.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly ILibraryRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthorController> _logger;

        public AuthorController(ILibraryRepository repository, IMapper mapper, ILogger<AuthorController> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<AuthorViewModel>> Get()
        {
            try
            {
                return Ok(_repository.GetAllAuthors());
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception: {e}");
                return BadRequest("failed to get authors");
            }
        }

        [HttpGet("{id:int}")]
        public ActionResult<AuthorViewModel> Get(int id)
        {
            try
            {
                return Ok(_mapper.Map<Author, AuthorViewModel>(_repository.GetAuthor(id)));
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception: {e}");
                return BadRequest("failed to get authors");
            }
        }

        [HttpPost]
        public ActionResult<AuthorViewModel> Post([FromBody] AuthorViewModel author)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var authorModel = _mapper.Map<AuthorViewModel, Author>(author);
                    _repository.AddNewAuthor(authorModel);

                    return Created($"/api/author/{authorModel.Id}", author);
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception: {e}");
                return BadRequest("failed to add new author");
            }
        }
    }
}