using Library.Domain;
using Library.Domain.Common;
using Library.Domain.Repositories;

namespace Library.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LibraryContext _ctx;
        private readonly IEventDispatcher _eventDispatcher;

        public UnitOfWork(
            LibraryContext ctx, 
            IEventDispatcher eventDispatcher, 
            IEntityFactory entityFactory)
        {
            _ctx = ctx;
            _eventDispatcher = eventDispatcher;
            Books = new BooksRepository(ctx, entityFactory);
            Authors = new AuthorsRepository(ctx, entityFactory);
            Users = new UsersRepository(ctx, entityFactory);
        }
        
        public IBooksRepository Books { get; }

        public IAuthorsRepository Authors { get; }

        public IUsersRepository Users { get; }

        public void Dispose()
        {
            _ctx.SaveChanges();

            _eventDispatcher.RaiseDeferredEvents();
        }
    }
}