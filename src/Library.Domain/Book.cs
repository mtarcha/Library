using System;
using System.Collections.Generic;
using System.Linq;
using Library.Domain.Common;
using Library.Domain.Events;

namespace Library.Domain
{
    public class Book : Entity<Guid>, IAggregateRoot
    {
        private const double RateChangeTollerance = 0.01;

        private readonly List<BookRate> _rates;
        private readonly List<Author> _authors;
        
        internal Book(IEventDispatcher eventDispatcher, string name, DateTime date, string summary)
           : this(eventDispatcher, name, date, summary, null)
        { }

        internal Book(IEventDispatcher eventDispatcher, string name, DateTime date, string summary, byte[] picture)
           : this(eventDispatcher, Guid.NewGuid(), name, date, summary, picture, new List<BookRate>())
        { }

        internal Book(IEventDispatcher eventDispatcher, Guid id, string name, DateTime date, string summary, byte[] picture, IEnumerable<BookRate> rates)
            : base(id, eventDispatcher)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Id cannot be empty.", nameof(id));
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Name cannot be empty.", nameof(name));
            }

            if (string.IsNullOrEmpty(summary))
            {
                throw new ArgumentException("Summary cannot be empty.", nameof(summary));
            }

            if (date > DateTime.Now)
            {
                throw new ArgumentException("Only existing books are valid. Date must be today or less", nameof(date));
            }

            Id = id;
            Name = name;
            Date = date;
            Summary = summary;
            Picture = picture;

            _rates = rates.ToList();
            _authors = new List<Author>();

            EvaluateRate();
        }
        
        public string Name { get; }

        public double? Rate { get; private set; }

        public DateTime Date { get; }

        public string Summary { get; }

        public byte[] Picture { get; }

        public IReadOnlyList<BookRate> Rates => _rates;
        public IReadOnlyList<Author> Authors => _authors;
        
        public void SetRate(BookRate bookRate)
        {
            AddOrUpdateRate(bookRate);
            EvaluateRate();
        }

        public void AddAuthor(Author author)
        {
            if (!_authors.Exists(x => x.Id == author.Id))
            {
                _authors.Add(author);
                author.AddBook(this);
            }
        }

        private void AddOrUpdateRate(BookRate rate)
        {
            var existing = _rates.SingleOrDefault(x => x.User.Id == rate.User.Id);
            if (existing != null)
            {
                existing.ChangeRate(rate.Rate);
            }
            else
            {
                _rates.Add(rate);
            }
        }

        private void EvaluateRate()
        {
            if (_rates.Any())
            {
                var rate = _rates.Average(x => x.Rate);

                if (!Rate.HasValue || Math.Abs(rate - Rate.Value) > RateChangeTollerance)
                {
                    var initialization = !Rate.HasValue;

                    Rate = rate;

                    if (!initialization)
                    {
                        RaiseEventDeferred(new BookRateChangedEvent(rate, Id));
                    }
                }
            }
        }
    }
}
