﻿using System;
using Library.Domain;
using Library.Domain.Common;
using Library.Infrastructure.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace Library.Infrastructure.Data
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly LibraryContext _ctx;
        private readonly IEventDispatcher _eventDispatcher;
        private readonly IEntityFactory _entityFactory;
        private readonly UserManager<UserEntity> _userManager;
        private readonly SignInManager<UserEntity> _signInManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;

        public UnitOfWorkFactory(
            LibraryContext ctx,
            IEventDispatcher eventDispatcher,
            IEntityFactory entityFactory, 
            UserManager<UserEntity> userManager, 
            SignInManager<UserEntity> signInManager, 
            RoleManager<IdentityRole<Guid>> roleManager)
        {
            _ctx = ctx;
            _eventDispatcher = eventDispatcher;
            _entityFactory = entityFactory;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IUnitOfWork Create()
        {
            return new UnitOfWork(_ctx, _eventDispatcher, _entityFactory, _userManager, _signInManager, _roleManager);
        }
    }
}