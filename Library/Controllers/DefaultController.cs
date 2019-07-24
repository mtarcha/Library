using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Library.Data;
using Library.Data.Entities;
using Library.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.Controllers
{
    public class DefaultController : Controller
    {
        public const int BooksOnPage = 8;
        private readonly ILibraryRepository _repository;
        private readonly IMapper _mapper;

        public DefaultController(ILibraryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index([FromQuery(Name = "pattern")]string search = "", [FromQuery(Name = "page")]int page = 1)
        {
            var pattern = search ?? string.Empty;
            var totalCount = _repository.Get(x => x.Name.Contains(pattern)).Count();

            var totalPages = (int)Math.Ceiling(totalCount / (decimal)BooksOnPage);

            if (page < 1 || (page > totalPages && totalPages > 0))
            {
                return NotFound($"Invalid page '{page}'! Should be in range from 1 to {totalPages}");
            }

            var books = _repository.Get(x => x.Name.Contains(pattern)).Skip((page - 1) * BooksOnPage).Take(BooksOnPage).ToList();

            var vm = new BooksViewModel
            {
                TotalBooksCount = totalCount,
                Pagination = new PaginationViewModel(totalPages, totalPages, page),
                BooksOnPage = _mapper.Map<IEnumerable<Book>, IEnumerable<BookViewModel>>(books),
                SearchPattern = pattern
            };

            return View(vm);
        }
    }
}