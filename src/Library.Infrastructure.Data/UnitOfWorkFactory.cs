using Library.Domain;
using Library.Domain.Common;

namespace Library.Infrastructure.Data
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly LibraryContext _ctx;
        private readonly IEventDispatcher _eventDispatcher;
        private readonly IEntityFactory _entityFactory;

        public UnitOfWorkFactory(
            LibraryContext ctx,
            IEventDispatcher eventDispatcher,
            IEntityFactory entityFactory)
        {
            _ctx = ctx;
            _eventDispatcher = eventDispatcher;
            _entityFactory = entityFactory;
        }

        public IUnitOfWork Create()
        {
            return new UnitOfWork(_ctx, _eventDispatcher, _entityFactory);
        }
    }
}