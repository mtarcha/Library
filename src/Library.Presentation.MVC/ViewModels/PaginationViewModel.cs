using System.Collections.Generic;
using System.Linq;

namespace Library.Presentation.MVC.ViewModels
{
    public class PaginationViewModel
    {
        private readonly int _totalPagesCount;
        private readonly int _page;

        public PaginationViewModel(int totalPagesCount, int page)
        {
            _totalPagesCount = totalPagesCount;
          
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
            var pages = Enumerable.Range(1, _totalPagesCount);

            Pages = pages.Select(x => new PageViewModel(x, x == _page, x != _page));
            Next = new PageViewModel(_page + 1 > _totalPagesCount ? _totalPagesCount : _page + 1, false, _page + 1 <= _totalPagesCount);
            Previous = new PageViewModel(_page - 1 < 1 ? 1 : _page - 1, false, _page - 1 >= 1);
        }
    }
}