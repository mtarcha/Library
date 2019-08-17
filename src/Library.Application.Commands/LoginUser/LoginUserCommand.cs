using Library.Application.Commands.Common;
using MediatR;

namespace Library.Application.Commands.LoginUser
{
    public class LoginUserCommand : IRequest<User>
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}