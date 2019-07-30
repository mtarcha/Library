using System;

namespace Library.Domain
{
    public class BookRate : Entity<Guid>
    {
        public BookRate(User user, int rate) 
            : this(Guid.NewGuid(), user, rate)
        {
        }

        public BookRate(Guid id, User user, int rate) : base(id)
        {
            if (rate < 0 || rate > 10)
            {
                throw new AggregateException("Rate must be in [0;10] range.");
            }

            User = user;
            Rate = rate;
        }

        public User User { get; }
        public int Rate { get; }
    }
}