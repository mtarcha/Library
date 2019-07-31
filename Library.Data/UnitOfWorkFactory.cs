﻿using Library.Data.Entities;
using Library.Data.Internal;
using Library.Domain;
using Microsoft.AspNetCore.Identity;

namespace Library.Data
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly LibraryContext _ctx;
        private readonly UserManager<UserEntity> _userManager;
        private readonly SignInManager<UserEntity> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UnitOfWorkFactory(
            LibraryContext ctx, 
            UserManager<UserEntity> userManager, 
            SignInManager<UserEntity> signInManager, 
            RoleManager<IdentityRole> roleManager)
        {
            _ctx = ctx;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IUnitOfWork Create()
        {
            return new UnitOfWork(_ctx, _userManager, _signInManager, _roleManager);
        }
    }
}