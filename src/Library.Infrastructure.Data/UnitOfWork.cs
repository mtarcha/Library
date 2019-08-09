using System;
using Library.Domain;
using Library.Domain.Common;
using Library.Infrastructure.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace Library.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LibraryContext _ctx;
        private readonly IEventDispatcher _eventDispatcher;

        public UnitOfWork(
            LibraryContext ctx, 
            IEventDispatcher eventDispatcher, 
            IEntityFactory entityFactory, 
            UserManager<UserEntity> userManager, 
            SignInManager<UserEntity> signInManager, 
            RoleManager<IdentityRole<Guid>> roleManager)
        {
            _ctx = ctx;
            _eventDispatcher = eventDispatcher;
            Books = new BooksRepository(ctx, entityFactory);
            Authors = new AuthorsRepository(ctx, entityFactory);
            Users = new UsersRepository(ctx, entityFactory, userManager, signInManager, roleManager);
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