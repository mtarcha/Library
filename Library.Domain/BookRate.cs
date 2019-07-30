using System;

namespace Library.Domain
{
    public class BookRate : Entity<Guid>
    {
        public const int MinValue = 1;
        public const int MaxValue = 5;

        public BookRate(User user, int rate) 
            : this(Guid.NewGuid(), user, rate)
        {
        }

        public BookRate(Guid id, User user, int rate) : base(id)
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