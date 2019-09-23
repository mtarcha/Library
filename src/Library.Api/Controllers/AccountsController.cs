using System;
using System.Threading.Tasks;
using AutoMapper;
using Library.Api.ViewModels;
using Library.Application.Commands.AddUser;
using Library.Application.Commands.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class AccountsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public AccountsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add(AddUserViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var command = _mapper.Map<AddUserViewModel, AddUserCommand>(model);
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [HttpGet("{id}")]
        Task<User> GetUser(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}