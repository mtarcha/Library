using System;
using Library.Domain.Common;

namespace Library.Domain.Entities
{
    public class BookRate : Entity<Guid>
    {
        public const int MinValue = 1;
        public const int MaxValue = 5;

        public BookRate(IEventDispatcher eventDispatcher, User user, int rate) 
            : this(Guid.NewGuid(), eventDispatcher, user, rate)
        {
        }

        public BookRate(Guid id, IEventDispatcher eventDispatcher, User user, int rate) 
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
                throw new ArgumentOutOfRangeException($"Rate must be in [{MinValue};{MaxValue}] range.");
            }

            Rate = rate;
        }
    }
}