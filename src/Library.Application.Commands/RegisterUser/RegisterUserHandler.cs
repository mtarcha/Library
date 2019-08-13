using System;
using System.Threading;
using System.Threading.Tasks;
using Library.Domain;
using MediatR;

namespace Library.Application.Commands.RegisterUser
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, RegisterUserResult>
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly IEntityFactory _entityFactory;

        public RegisterUserHandler(IUnitOfWorkFactory unitOfWorkFactory, IEntityFactory entityFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _entityFactory = entityFactory;
        }

        public async Task<RegisterUserResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            using (var uow = _unitOfWorkFactory.Create())
            {
                User user = null;
                try
                {
                    user = _entityFactory.CreateUser(request.UserName, request.DateOfBirth, Role.User);
                    user.SetPassword(request.Password);
                    await uow.Users.CreateAsync(user, cancellationToken);
                }
                catch (Exception e)
                {
                    return new RegisterUserResult(e);
                }

                return new RegisterUserResult(user.Id);
            }
        }
    }
}