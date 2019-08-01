﻿using Library.Domain;
using Library.Infrastucture.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace Library.Infrastucture.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LibraryContext _ctx;

        public UnitOfWork(LibraryContext ctx, EntityFactory entityFactory, UserManager<UserEntity> userManager, SignInManager<UserEntity> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _ctx = ctx;
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
        }
    }
}