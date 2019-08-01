namespace Library.Domain.Events
{
    public interface IDomainEventVisitor<out T>
    {
        T VisitBookRateChangedEvent(BookRateChanged bookRateChangedEvent);
    }
}