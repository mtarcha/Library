using System;
using System.Threading;
using System.Threading.Tasks;
using Library.Application.Common;
using Library.Domain;
using MediatR;

namespace Library.Application.Commands.LoginUser
{
    public class LoginUserHandler : IRequestHandler<LoginUserCommand, RequestResult>
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public LoginUserHandler(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public async Task<RequestResult> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            using (var uow = _unitOfWorkFactory.Create())
            {
                try
                {
                    await uow.Users.LoginAsync(request.UserName, request.Password, request.RememberMe, cancellationToken);
                }
                catch (Exception e)
                {
                    return new RequestResult(e);
                }
                
                return RequestResult.Success;
            }
        }
    }
}