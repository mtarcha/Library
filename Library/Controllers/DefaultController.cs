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
        private readonly ILibraryRepository _repository;
        private readonly IMapper _mapper;

        public DefaultController(ILibraryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(_mapper.Map<IEnumerable<Book>, IEnumerable<BookViewModel>>(_repository.GetAllBooks()));
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignIn(SignInViewModel signInViewModel)
        {
            if (ModelState.IsValid)
            {

            }

            return RedirectToAction("Index");
        }

    }
}