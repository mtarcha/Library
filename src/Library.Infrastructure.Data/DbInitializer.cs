using System;
using System.Linq;
using Library.Domain;
using Library.Infrastructure.Core;

namespace Library.Infrastructure.Data
{
    public class DbInitializer : IStorageSeeder
    {
        private readonly LibraryContext _ctx;
        private readonly IEntityFactory _entityFactory;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public DbInitializer(LibraryContext ctx, IEntityFactory entityFactory, IUnitOfWorkFactory unitOfWorkFactory)
        {
            _ctx = ctx;
            _entityFactory = entityFactory;
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
                    var ivanko = _entityFactory.CreateAuthor("Ivan", "Ivchenko", new LifePeriod(new DateTime(1988, 1, 11)));
                    var slavko = _entityFactory.CreateAuthor("Myroslava", "Tarcha", new LifePeriod(new DateTime(1993, 5, 5)));

                    var astrid = _entityFactory.CreateAuthor(
                        "Astrid", 
                        "Lindgren",
                        new LifePeriod(new DateTime(1907, 11, 14), new DateTime(2002, 1, 14)));

                    for (var i = 1; i < 10; i++)
                    {
                        var book = _entityFactory.CreateBook(
                            "Karlsson-on-the-Roof " + i, 
                            new DateTime(2016, 09, 08),
                            "Karlsson-on-the-Roof is a character who figures in a series of children's books by the Swedish author Astrid Lindgren");

                        book.AddAuthor(astrid);
                    }

                    for (var i = 1; i < 15; i++)
                    {
                        var book = _entityFactory.CreateBook("Adventures of Vichyk and Tarchavka" + i, new DateTime(2016, 09, 08), "Adventures of Vichyk and Tarchavka " + i);
                        book.AddAuthor(ivanko);
                        book.AddAuthor(slavko);
                    }

                    unitOfWork.Authors.Create(ivanko);
                    unitOfWork.Authors.Create(slavko);
                    unitOfWork.Authors.Create(astrid);
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
                    user = _entityFactory.CreateUser(name, new DateTime(1993, 5, 5), Role.Admin);
                    user.SetPassword("K.,k. ;bnnz1");
                    unitOfWork.Users.Create(user);
                }
            }
        }
    }
}