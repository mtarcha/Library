using Library.Data.Entities;
using Library.Data.Internal;
using Library.Domain;
using Microsoft.AspNetCore.Identity;

namespace Library.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LibraryContext _ctx;

        public UnitOfWork(LibraryContext ctx, UserManager<UserEntity> userManager, SignInManager<UserEntity> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _ctx = ctx;
            Books = new BooksRepository(ctx);
            Authors = new AuthorsRepository(ctx);
            Users = new UsersRepository(ctx, userManager, signInManager, roleManager);
        }
        
        public IBooksRepository Books { get; }

        public IAuthorsRepository Authors { get; }

        public IUsersRepository Users { get; }

        public void Dispose()
        {
            _ctx.SaveChanges();
        }
    }
}