using System.Threading;
using System.Threading.Tasks;
using Library.Application.Commands.Common;
using Library.Domain;
using MediatR;

namespace Library.Application.Commands.LoginUser
{
    public class LoginUserHandler : IRequestHandler<LoginUserCommand, User>
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public LoginUserHandler(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public async Task<User> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            using (var uow = _unitOfWorkFactory.Create())
            {
                var user = await uow.Users.LoginAsync(request.UserName, request.Password, request.RememberMe, cancellationToken);

                return new User {Id = user.Id, UserName = user.UserName};
            }
        }
    }
}