using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Library.Presentation.MVC.Models;
using RestEase;

namespace Library.Presentation.MVC.Clients
{
    public interface IBooksClient
    {
        [Get("books")]
        Task<Response<SearchBooksResult>> Get(string pattern, int skipCount, int takeCount);
        
        [Post("books")]
        Task<Response<Book>> Create([Header("Authorization")] string authorization, [Body] CreateBookModel bookViewModel);
        
        [Get("books/{id}")]
        Task<Response<Book>> GetBook([Header("Authorization")] string authorization, [Path("id")] Guid id);

        [Put("books")]
        Task<Response<Book>> UpdateBook([Header("Authorization")] string authorization, [Body] UpdateBookModel bookViewModel);

        [Put("books/set_rate")]
        Task<Response<Book>> SetRate([Header("Authorization")] string authorization, [Body] SetRateModel setRateViewModel);
    }
}