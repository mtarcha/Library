using System.Threading;
using System.Threading.Tasks;
using Library.Domain;
using Library.Domain.Common;
using Library.Domain.Events;
using MediatR;

namespace Library.Application.EventHandling.Handlers
{
    public sealed class FavoriteBookAddedEventHandler : IDomainEventHandler<FavoriteBookAddedEvent>
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly IMediator _mediator;

        public FavoriteBookAddedEventHandler(IUnitOfWorkFactory unitOfWorkFactory, IMediator mediator)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _mediator = mediator;
        }

        public async Task Handle(FavoriteBookAddedEvent notification, CancellationToken cancellationToken)
        {
            using (var uow = _unitOfWorkFactory.Create())
            {
                var book = await uow.Books.GetByIdAsync(notification.BookId, cancellationToken);

                cancellationToken.ThrowIfCancellationRequested();

                var followers = await uow.Users.GetFollowersAsync(notification.UserId, cancellationToken);

                foreach (var follower in followers)
                {
                    follower.AddRecommendedBook(book);
                }
            }
        }
    }
}