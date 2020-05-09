using System;
using System.Threading.Tasks;
using Library.Presentation.MVC.Models;
using RestEase;

namespace Library.Presentation.MVC.Clients
{
    public interface IBooksClient
    {
        //[Get("books")]
        Task<Response<SearchBooksResult>> Get(string pattern, int skipCount, int takeCount);
        
       // [Post("books")]
        Task<Response<Book>> Create([Body] CreateBookModel bookViewModel);
        
        //[Get("books/{id}")]
        Task<Response<Book>> GetBook([Path("id")] Guid id);

        //[Put("books")]
        Task<Response<Book>> UpdateBook([Body] UpdateBookModel bookViewModel);

        //[Put("books/set_rate")]
        Task<Response<Book>> SetRate([Body] SetRateModel setRateViewModel);
    }
}