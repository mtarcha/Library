using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Data.Entities;
using Library.Data.Internal;
using Library.Domain;
using Microsoft.AspNetCore.Identity;

namespace Library.Data
{
    public class DbInitializer
    {
        private readonly LibraryContext _ctx;
        private readonly UserManager<UserEntity> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(LibraryContext ctx, UserManager<UserEntity> userManager, RoleManager<IdentityRole> roleManager)
        {
            _ctx = ctx;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task Initialize()
        {
            _ctx.Database.EnsureCreated();
            await SeedLibrary();
            await SeedRoles();
        }

        public Task SeedLibrary()
        {
            return Task.Run(() =>
            {
                if (!_ctx.Authors.Any() || !_ctx.Books.Any())
                {
                    var ivanko = new AuthorEntity
                    {
                        ReferenceId = Guid.NewGuid(),
                        Name = "Ivan",
                        SurName = "Ivchenko",
                        DateOfBirth = new DateTime(1988, 1, 11)
                    };

                    var slavko = new AuthorEntity
                    {
                        ReferenceId = Guid.NewGuid(),
                        Name = "Slavka",
                        SurName = "Tarcha",
                        DateOfBirth = new DateTime(1993, 5, 5)
                    };

                    _ctx.Authors.AddRange(ivanko, slavko);

                    for (var i = 1; i < 25; i++)
                    {
                        var book = new BookEntity()
                        {
                            ReferenceId = Guid.NewGuid(),
                            Name = "Пригоди Вівчика й Тарчавки " + i,
                            Date = DateTime.Now,
                            Rate = 0,
                            Summary = "Книга пригод про Вівчика й Тарчавку " + i,
                        };

                        book.Authors = new List<BookAuthorEntity>
                        {
                            new BookAuthorEntity() { Book = book, Author = ivanko },
                            new BookAuthorEntity() { Book = book, Author = slavko }
                        };

                        _ctx.Books.Add(book);
                    }

                    for (var i = 1; i < 5; i++)
                    {
                        var book = new BookEntity()
                        {
                            ReferenceId = Guid.NewGuid(),
                            Name = "Пригоди Вівчика " + i,
                            Date = DateTime.Now,
                            Rate = 0,
                            Summary = "Книга пригод про Вівчика до зустрічі з Тарчавкою " + i,
                        };

                        book.Authors = new List<BookAuthorEntity>
                        {
                            new BookAuthorEntity() { Book = book, Author = ivanko }
                        };

                        _ctx.Books.Add(book);
                    }

                    for (var i = 1; i < 5; i++)
                    {
                        var book = new BookEntity()
                        {
                            ReferenceId = Guid.NewGuid(),
                            Name = "Пригоди Тарчавки " + i,
                            Date = DateTime.Now,
                            Rate = 0,
                            Summary = "Книга пригод про Тарчавку до зустрічі з Вівчиком " + i,
                        };

                        book.Authors = new List<BookAuthorEntity>
                        {
                            new BookAuthorEntity() { Book = book, Author = slavko }
                        };

                        _ctx.Books.Add(book);
                    }

                    _ctx.SaveChanges();
                }
            });

        }

        public async Task SeedRoles()
        {
            string[] roleNames = { Roles.Admin, Roles.User };
            foreach (var roleName in roleNames)
            {
                var roleExist = await _roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            var name = "AdminMyroslava";
            var user = await _userManager.FindByNameAsync(name);
            if (user == null)
            {
                var poweruser = new UserEntity()
                {
                    UserName = name,
                    ReferenceId = Guid.NewGuid(),
                    DateOfBirth = new DateTime(1993, 5, 5)
                };

                var createPowerUser = await _userManager.CreateAsync(poweruser, "K.,k. ;bnnz1");
                if (createPowerUser.Succeeded)
                {
                    await _userManager.AddToRoleAsync(poweruser, Roles.Admin);
                }
            }
        }
    }
}