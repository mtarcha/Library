using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Library.Domain;
using Library.Domain.Entities;

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

        public async Task SeedAsync(CancellationToken token)
        {
            _ctx.Database.EnsureCreated();
            await SeedLibraryAsync(token);
        }

        public async Task SeedLibraryAsync(CancellationToken token)
        {
            if (!_ctx.Authors.Any() || !_ctx.Books.Any())
            {
                using (var unitOfWork = _unitOfWorkFactory.Create())
                {
                    var ivanko = _entityFactory.CreateAuthor(
                        "Ivan", 
                        "Ivchenko", 
                        new LifePeriod(new DateTime(1988, 1, 11)));

                    var slavko = _entityFactory.CreateAuthor(
                        "Myroslava", 
                        "Tarcha", 
                        new LifePeriod(new DateTime(1993, 5, 5)));

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

                    for (var i = 1; i < 10; i++)
                    {
                        var book = _entityFactory.CreateBook(
                            "Adventures of Vivchyk and Tarchavka" + i, 
                            new DateTime(2016, 09, 08), 
                            "Adventures of Vivchyk and Tarchavka " + i);

                        book.AddAuthor(ivanko);
                        book.AddAuthor(slavko);
                    }

                    for (var i = 1; i < 10; i++)
                    {
                        var book = _entityFactory.CreateBook(
                            "Adventures of Vivchyk" + i,
                            new DateTime(2016, 09, 08),
                            "Adventures of Vivchyk before he has met Tarchavka " + i);

                        book.AddAuthor(ivanko);
                    }

                    for (var i = 1; i < 10; i++)
                    {
                        var book = _entityFactory.CreateBook(
                            "Adventures of Tarchavka" + i,
                            new DateTime(2016, 09, 08),
                            "Adventures of Tarchavka before she has met Vivchyk " + i);

                        book.AddAuthor(slavko);
                    }

                    await unitOfWork.Authors.CreateAsync(ivanko, token);
                    await unitOfWork.Authors.CreateAsync(slavko, token);
                    await unitOfWork.Authors.CreateAsync(astrid, token);
                }
            }
        }
    }
}