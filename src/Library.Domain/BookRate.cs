using System;
using Library.Domain.Common;

namespace Library.Domain
{
    public class BookRate : Entity<Guid>
    {
        public const int MinValue = 1;
        public const int MaxValue = 5;

        public BookRate(EventDispatcher eventDispatcher, User user, int rate) 
            : this(Guid.NewGuid(), eventDispatcher, user, rate)
        {
        }

        public BookRate(Guid id, EventDispatcher eventDispatcher, User user, int rate) 
            : base(id, eventDispatcher)
        {
            User = user;

            ChangeRate(rate);
        }

        public User User { get; }
        public int Rate { get; private set; }

        public void ChangeRate(int rate)
        {
            if (rate < MinValue || rate > MaxValue)
            {
                throw new AggregateException($"Rate must be in [{MinValue};{MaxValue}] range.");
            }

            Rate = rate;
        }
    }
}