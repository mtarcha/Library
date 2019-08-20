using System.Threading.Tasks;
using AutoMapper;
using Library.Api.ViewModels;
using Library.Application.Commands.LoginUser;
using Library.Application.Commands.LogoutUser;
using Library.Application.Commands.RegisterUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Controllers
{
    [ApiController]
    [Route("api/accounts")]
    public class AccountsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public AccountsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var command = _mapper.Map<RegisterViewModel, RegisterUserCommand>(model);
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var command = _mapper.Map<LoginViewModel, LoginUserCommand>(model);
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [HttpPost("logoff")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            var result = await _mediator.Send(new LogoutCommand());
            return Ok(result);
        }
    }
}