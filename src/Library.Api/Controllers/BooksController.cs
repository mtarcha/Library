using System;
using System.Threading.Tasks;
using AutoMapper;
using Library.Api.ViewModels;
using Library.Application.Commands.CreateBook;
using Library.Application.Commands.SetBookRate;
using Library.Application.Commands.UpdateBook;
using Library.Application.Queries.GetBook;
using Library.Application.Queries.GetBooks;
using Library.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
        [AllowAnonymous]
        public async Task<IActionResult> Get(
            [FromQuery(Name = "pattern")] string search, 
            [FromQuery(Name = "skipCount")] int skipCount, 
            [FromQuery(Name = "takeCount")] int takeCount)
        {
            //if (!ModelState.IsValid)
            //    return BadRequest(ModelState);

            var query = new GetBooksQuery
            {
                SearchPattern = search ?? "",
                SkipCount = skipCount,
                TakeCount = takeCount
            };

            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = Role.UserRoleName + "," + Role.AdminRoleName)]
        public async Task<IActionResult> Create(CreateBookViewModel bookViewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var command = _mapper.Map<CreateBookViewModel, CreateBookCommand>(bookViewModel);
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook(Guid id)
        {
            var query = new GetBookQuery { BookId = id };
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [Authorize(Roles = Role.AdminRoleName)]
        [HttpPut]
        public async Task<IActionResult> UpdateBook(UpdateBookViewModel bookViewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var command = _mapper.Map<UpdateBookViewModel, UpdateBookCommand>(bookViewModel);
            var result = await _mediator.Send(command);
          
            return Ok(result);
        }

        [HttpPut("set_rate")]
        [Authorize(Roles = Role.UserRoleName + "," + Role.AdminRoleName)]
        public async Task<IActionResult> SetRate(SetRateViewModel setRateViewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var command = new SetBookRateCommand
            {
                UserId = setRateViewModel.UserId,
                BookId = setRateViewModel.BookId,
                Rate = setRateViewModel.Rate
            };

            var result = await _mediator.Send(command);

            return Ok(result);
        }
    }
}