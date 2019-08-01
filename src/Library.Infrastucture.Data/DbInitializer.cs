using System;
using System.Linq;
using Library.Domain;
using Library.Infrastucture.Core;

namespace Library.Infrastucture.Data
{
    public class DbInitializer : IStorageSeeder
    {
        private readonly LibraryContext _ctx;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public DbInitializer(LibraryContext ctx, IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public void Seed()
        {
            _ctx.Database.EnsureCreated();
            SeedLibrary();
            SeedRoles();
        }

        public void SeedLibrary()
        {
            if (!_ctx.Authors.Any() || !_ctx.Books.Any())
            {
                using (var unitOfWork = _unitOfWorkFactory.Create())
                {
                    var ivanko = new Author("Ivan", "Ivchenko", new LifePeriod(new DateTime(1988, 1, 11)));
                    var slavko = new Author("Myroslava", "Tarcha", new LifePeriod(new DateTime(1993, 5, 5)));

                    unitOfWork.Authors.Create(ivanko);
                    unitOfWork.Authors.Create(slavko);

                    for (var i = 1; i < 25; i++)
                    {
                        var book = new Book("Пригоди Вівчика й Тарчавки " + i, new DateTime(2016, 09, 08), "Книга пригод про Вівчика й Тарчавку " + i);

                        book.AddAuthor(ivanko);
                        book.AddAuthor(slavko);
                    }

                    for (var i = 1; i < 5; i++)
                    {
                        var book = new Book("Пригоди Вівчика " + i, new DateTime(2016, 09, 08), "Книга пригод про Вівчика до зустрічі з Тарчавкою " + i);
                        book.AddAuthor(ivanko);
                    }

                    for (var i = 1; i < 5; i++)
                    {
                        var book = new Book("Пригоди Тарчавки " + i, new DateTime(2016, 09, 08), "Книга пригод про Тарчавку до зустрічі з Вівчиком " + i);
                        book.AddAuthor(slavko);
                    }
                }
            }
        }

        public void SeedRoles()
        {
            using (var unitOfWork = _unitOfWorkFactory.Create())
            {
                var roles = new [] { Role.User, Role.Admin };
                foreach (var role in roles)
                {
                    unitOfWork.Users.CreateRoleIfNotExists(role);
                }

                var name = "AdminMyroslava";
                var user = unitOfWork.Users.GetByName(name);
                if (user == null)
                {
                    user = new User(name, Role.Admin);
                    user.SetPassword("K.,k. ;bnnz1");
                    unitOfWork.Users.Create(user);
                }
            }
        }
    }
}