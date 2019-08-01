using System.Collections.Generic;
using System.Linq;

namespace Library.Presentation.MVC.ViewModels
{
    public class PaginationViewModel
    {
        private readonly int _totalPagesCount;
        private readonly int _paginationCount;
        private readonly int _page;

        public PaginationViewModel(int totalPagesCount, int paginationCount, int page)
        {
            _totalPagesCount = totalPagesCount;
            _paginationCount = paginationCount;
            _page = page;

            CurrenPageNumber = page;

            Initialize();
        }

        public int CurrenPageNumber { get; }
        
        public PageViewModel Next { get; private set; }

        public IEnumerable<PageViewModel> Pages { get; private set; }

        public PageViewModel Previous { get; private set; }

        private void Initialize()
        {
            var pages =
                Enumerable
                  .Range(_page - _paginationCount, _paginationCount * 2)
                  .Where(x => x > 0)
                  .Where(x => x <= _totalPagesCount)
                  .ToList();

            while (pages.Count > _paginationCount && pages.Count != 1)
            {
                pages.RemoveAt(pages.Count(x => x < _page) > pages.Count(x => x > _page) ? 0 : pages.Count - 1);
            }

            Pages = pages.Select(x => new PageViewModel(x, x == _page, x != _page));
            Next = new PageViewModel(_page + 1 > _totalPagesCount ? _totalPagesCount : _page + 1, false, _page + 1 <= _totalPagesCount);
            Previous = new PageViewModel(_page - 1 < 1 ? 1 : _page - 1, false, _page - 1 >= 1);
        }
    }
}