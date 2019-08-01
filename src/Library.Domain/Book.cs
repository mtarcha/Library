using System;
using System.Collections.Generic;
using System.Linq;
using Library.Domain.Common;

namespace Library.Domain
{
    public class Book : Entity<Guid>, IAggregateRoot
    {
        private const double RateChangeTollerance = 0.01;

        private readonly List<BookRate> _rates;
        private readonly List<Author> _authors;

        public Book(string name, DateTime date, string summary)
           : this(Guid.NewGuid(), name, date, summary, null)
        { }

        public Book(string name, DateTime date, string summary, byte[] picture)
           : this(Guid.NewGuid(), name, date, summary, picture)
        { }

        public Book(Guid id, string name, DateTime date, string summary, byte[] picture)
            : base(id)
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
            Rate = 0;

            _rates = new List<BookRate>();
            _authors = new List<Author>();
        }
        
        public string Name { get; }

        public double Rate { get; private set; }

        public DateTime Date { get; }

        public string Summary { get; }

        public byte[] Picture { get; }

        public IReadOnlyList<BookRate> Rates => _rates;
        public IReadOnlyList<Author> Authors => _authors;
        
        // todo: how update DB????
        public void SetRate(User user, int rate)
        {
            AddOrUpdateRate(new BookRate(user, rate));
            EvaluateRate();
        }

        public void SetRates(IEnumerable<BookRate> rates)
        {
            foreach (var rate in rates)
            {
                AddOrUpdateRate(rate);
            }
            
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
            double rate = 0;

            if (_rates.Any())
            {
                rate = _rates.Average(x => x.Rate);
            }

            if (Math.Abs(rate - Rate) > RateChangeTollerance)
            {
                Rate = rate;

                // todo: raise domain event
            }
        }
    }
}
