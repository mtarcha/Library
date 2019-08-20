using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Library.Presentation.MVC.Models;
using RestEase;

namespace Library.Presentation.MVC.Clients
{
    public interface IBooksClient
    {

        [Get("api/books")]
        Task<Response<IEnumerable<Book>>> Get(string search, int skipCount, int takeCount);


        [Post("api/books")]
        Task<Response<Book>> Create([Body] CreateBookModel bookViewModel);


        [Get("{id}")]
        Task<Response<Book>> GetBook(Guid id);

        [Put]
        Task<Response<Book>> UpdateBook([Body] UpdateBookModel bookViewModel);

        [Put("set_rate")]
        Task<Response<Book>> SetRate([Body] SetRateModel setRateViewModel);
    }
}