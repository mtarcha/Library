using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Data;
using Library.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.Controllers
{
    public class DefaultController : Controller
    {
        private readonly ILibraryRepository _repository;
        
        public DefaultController(ILibraryRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(_repository.GetAllBooksIncludingAuthors().Select(book => new BookDescription(
                book.Name, 
                book.Date, 
                book.Authors.Select(author => new Author(author.Author.Name, author.Author.SurName)).ToArray(), 
                book.Summary, 
                10)));
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignIn(SignInInformation info)
        {
            if (ModelState.IsValid)
            {

            }

            return RedirectToAction("Index");
        }

    }
}