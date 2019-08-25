using System.Collections.Generic;

namespace Library.Presentation.MVC.Models
{
    public class SearchBooksResult
    {
        public SearchBooksResult(IEnumerable<Book> books, int totalBooksCount, int foundBooksCount)
        {
            Books = books;
            TotalBooksCount = totalBooksCount;
            FoundBooksCount = foundBooksCount;
        }

        public int TotalBooksCount { get; set; }

        public int FoundBooksCount { get; set; }

        public IEnumerable<Book> Books { get; set; }
    }
}