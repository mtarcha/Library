using System.Threading;
using System.Threading.Tasks;
using Library.Domain;
using MediatR;

namespace Library.Application.Commands.LogoutUser
{
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand>
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public LogoutCommandHandler(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }
        public async Task<Unit> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            using (var uow = _unitOfWorkFactory.Create())
            {
                await uow.Users.LogoutAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}