using System.Threading;
using System.Threading.Tasks;
using Library.Domain;
using Library.Domain.Entities;
using MediatR;
using User = Library.Application.Commands.Common.User;

namespace Library.Application.Commands.RegisterUser
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, User>
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly IEntityFactory _entityFactory;

        public RegisterUserHandler(IUnitOfWorkFactory unitOfWorkFactory, IEntityFactory entityFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _entityFactory = entityFactory;
        }

        public async Task<User> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            using (var uow = _unitOfWorkFactory.Create())
            {
                var user = _entityFactory.CreateUser(request.UserName, request.DateOfBirth, Role.User);
                user.SetPassword(request.Password);
                await uow.Users.CreateAsync(user, cancellationToken);

                return new User { Id = user.Id, UserName = user.UserName };
            }
        }
    }
}