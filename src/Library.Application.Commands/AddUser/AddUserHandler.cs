using System.Threading;
using System.Threading.Tasks;
using Library.Domain;
using MediatR;
using User = Library.Application.Commands.Common.User;

namespace Library.Application.Commands.AddUser
{
    public class AddUserHandler : IRequestHandler<AddUserCommand, User>
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly IEntityFactory _entityFactory;

        public AddUserHandler(IUnitOfWorkFactory unitOfWorkFactory, IEntityFactory entityFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _entityFactory = entityFactory;
        }

        public async Task<User> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            using (var uow = _unitOfWorkFactory.Create())
            {
                var user = _entityFactory.CreateUser(request.UserName, request.DateOfBirth);
              
                await uow.Users.CreateAsync(user, cancellationToken);

                return new User { Id = user.Id, UserName = user.UserName };
            }
        }
    }
}