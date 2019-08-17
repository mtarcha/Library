using System;
using System.Threading.Tasks;
using AutoMapper;
using Library.Api.ViewModels;
using Library.Application.Commands.CreateBook;
using Library.Application.Commands.SetBookRate;
using Library.Application.Commands.UpdateBook;
using Library.Application.Queries.GetBook;
using Library.Application.Queries.GetBooks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Controllers
{
    [ApiController]
    [Route("api/books")]
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
        public async Task<IActionResult> Get([FromQuery(Name = "pattern")] string search = "", [FromQuery(Name = "page")] int page = 1)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (page < 1)
            {
                return BadRequest($"Invalid page '{page}'! Should be 1 or more.");
            }

            var pattern = search ?? string.Empty;
            var query = new GetBooksQuery
            {
                SearchPattern = search,
                SkipCount = (page - 1) * BooksOnPage,
                TakeCount = BooksOnPage
            };

            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpPost("create")]
        //[Authorize(Roles = Role.UserRoleName + "," + Role.AdminRoleName)]
        public async Task<IActionResult> Create(CreateBookViewModel bookViewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var command = _mapper.Map<CreateBookViewModel, CreateBookCommand>(bookViewModel);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("{id}")]
        //[Authorize(Roles = Role.AdminRoleName)]
        public async Task<IActionResult> GetBook(Guid id)
        {
            var query = new GetBookQuery { BookId = id };

            var result = await _mediator.Send(query);
            // todo result should be book
            return Ok(result);
        }

        [HttpPost("update")]
        //[Authorize(Roles = Role.AdminRoleName)]
        public async Task<IActionResult> UpdateBook(UpdateBookViewModel bookViewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var command = _mapper.Map<UpdateBookViewModel, UpdateBookCommand>(bookViewModel);

            var result = await _mediator.Send(command);
            //todo: return book
            return Ok();
        }

        [HttpPost("setrate")]
        //[Authorize]
        public async Task<IActionResult> SetRate(SetRateViewModel setRateViewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //var user = _mapper.Send(GetUserCommand);

            var command = new SetBookRateCommand
            {
                UserName = "AdminMyroslava", // todo: fix
                BookId = setRateViewModel.BookId,
                Rate = setRateViewModel.Rate
            };

            var result = await _mediator.Send(command);

            return Ok(result);
        }
    }
}