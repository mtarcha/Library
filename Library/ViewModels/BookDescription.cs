using System;
using System.Collections.Generic;
namespace Library.ViewModels
{
    public class BookDescription
    {
        public BookDescription(string name, DateTime date, IEnumerable<Author> authors, string summary, int rate)
        {
            Name = name;
            Date = date;
            Authors = authors;
            Summary = summary;
            Rate = rate;
        }

        public string Name { get; }

        public DateTime Date { get; }

        public IEnumerable<Author> Authors { get; }

        public string Summary { get; }

        public int Rate { get; }
    }
}