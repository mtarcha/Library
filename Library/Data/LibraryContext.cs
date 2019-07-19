using System;
using Library.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.Data
{
    public class LibraryContext : DbContext
    {
        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>().ToTable("Authors").HasKey(p => p.Id);
            modelBuilder.Entity<Author>().Property(x => x.Name).IsRequired().HasMaxLength(1000);
            modelBuilder.Entity<Author>().Property(p => p.DateOfBirth).IsRequired();
            modelBuilder.Entity<Author>().Property(p => p.DateOfDeath).IsRequired(false);
            
            modelBuilder.Entity<Book>().ToTable("Books").HasKey(p => p.Id);
            modelBuilder.Entity<Book>().Property(p => p.Name).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Book>().Property(p => p.Summary).IsRequired().HasMaxLength(1000);
            modelBuilder.Entity<Book>().Property(p => p.Date).IsRequired();
           
            // many-to-many relationship. EF Core does not support it automatically.
            modelBuilder.Entity<BookAuthor>().HasKey(x => new { x.BookId, x.AuthorId });
            modelBuilder.Entity<BookAuthor>().HasOne(x => x.Book).WithMany(x => x.Authors).HasForeignKey(x => x.BookId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<BookAuthor>().HasOne(x => x.Author).WithMany(x => x.Books).HasForeignKey(x => x.AuthorId).OnDelete(DeleteBehavior.Cascade);
        }

        public DbSet<Book> Books { get; set; }

        public DbSet<Author> Author { get; set; }
    }
}