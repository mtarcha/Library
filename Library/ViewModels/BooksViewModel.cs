using System.Collections.Generic;

namespace Library.ViewModels
{
    public class BooksViewModel
    {
        public int TotalBooksCount { get; set; }

        public IEnumerable<BookViewModel> BooksOnPage { get; set; }

        public PaginationViewModel Pagination { get; set; }

        public string SearchPattern { get; set; }
    }
}