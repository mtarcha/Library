using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Library.Domain.Common;
using DomainEvents = Library.Domain.Events;

namespace Library.Business.EventHandling
{
    public class IntegrationEventsDispatcher 
        : IDomainEventHandler<DomainEvents.BookRateChangedEvent>
        , IDomainEventHandler<DomainEvents.RecommendedBookAddedEvent>
    {
        private readonly IMapper _mapper;
        readonly IEnumerable<IIntegrationEventHandler<RecommendedBookAddedEvent>> _recommendedBooksEventHandlers;
        readonly IEnumerable<IIntegrationEventHandler<BookRateChangedEvent>> _bookRateChangedEventHandlers;

        public IntegrationEventsDispatcher(
            IMapper mapper,
            IEnumerable<IIntegrationEventHandler<BookRateChangedEvent>> bookRateChangedEventHandlers,
            IEnumerable<IIntegrationEventHandler<RecommendedBookAddedEvent>> recommendedBooksEventHandlers)
        {
            _mapper = mapper;
            _recommendedBooksEventHandlers = recommendedBooksEventHandlers;
            _bookRateChangedEventHandlers = bookRateChangedEventHandlers;
        }

        public async Task Handle(DomainEvents.BookRateChangedEvent notification, CancellationToken cancellationToken)
        {
            if (_bookRateChangedEventHandlers != null)
            {
                var integrationEvent = _mapper.Map<DomainEvents.BookRateChangedEvent, BookRateChangedEvent>(notification);

                foreach (var bookRateChangedEventHandler in _bookRateChangedEventHandlers)
                {
                    await bookRateChangedEventHandler.Handle(integrationEvent, cancellationToken);
                }
            }
        }

        public async Task Handle(DomainEvents.RecommendedBookAddedEvent notification, CancellationToken cancellationToken)
        {
            if (_recommendedBooksEventHandlers != null)
            {
                var integrationEvent = _mapper.Map<DomainEvents.RecommendedBookAddedEvent, RecommendedBookAddedEvent>(notification);

                foreach (var recommendedBooksEventHandler in _recommendedBooksEventHandlers)
                {
                    await recommendedBooksEventHandler.Handle(integrationEvent, cancellationToken);
                }
            }
        }
    }
}