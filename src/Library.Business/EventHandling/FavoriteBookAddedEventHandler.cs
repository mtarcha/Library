using System.Threading;
using System.Threading.Tasks;
using Library.Domain;
using Library.Domain.Common;
using Library.Domain.Events;

namespace Library.Business.EventHandling
{
    public sealed class FavoriteBookAddedEventHandler : IDomainEventHandler<FavoriteBookAddedEvent>
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public FavoriteBookAddedEventHandler(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public Task Handle(FavoriteBookAddedEvent notification, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                using (var uow = _unitOfWorkFactory.Create())
                {
                    var book = uow.Books.GetById(notification.BookId);

                    cancellationToken.ThrowIfCancellationRequested();

                    foreach (var follower in uow.Users.GetFollowers(notification.UserId))
                    {
                        follower.AddRecommendedBook(book);
                    }
                }
            }, cancellationToken);
        }
    }
}