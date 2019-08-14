using Library.Application.Common;
using MediatR;

namespace Library.Application.Commands.LoginUser
{
    public class LoginUserCommand : IRequest<RequestResult>
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}