using Library.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Library.Data.Internal
{
    public sealed class LibraryContext : IdentityDbContext<UserEntity>
    {
        public LibraryContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<BookEntity> Books { get; set; }

        public DbSet<AuthorEntity> Authors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AuthorEntity>().ToTable("Authors").HasKey(p => p.Id);
            modelBuilder.Entity<AuthorEntity>().Property(x => x.Name).IsRequired().HasMaxLength(1000);
            modelBuilder.Entity<AuthorEntity>().Property(p => p.ReferenceId).IsRequired();
            modelBuilder.Entity<AuthorEntity>().Property(p => p.DateOfBirth).IsRequired();
            modelBuilder.Entity<AuthorEntity>().Property(p => p.DateOfDeath).IsRequired(false);

            modelBuilder.Entity<BookEntity>().ToTable("Books").HasKey(p => p.Id);
            modelBuilder.Entity<BookEntity>().Property(p => p.Name).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<BookEntity>().Property(p => p.Summary).IsRequired().HasMaxLength(1000);
            modelBuilder.Entity<BookEntity>().Property(p => p.ReferenceId).IsRequired();
            modelBuilder.Entity<BookEntity>().Property(p => p.Date).IsRequired();
            modelBuilder.Entity<BookEntity>().HasMany(x => x.Rates).WithOne(x => x.Book);

            // many-to-many relationship. EF Core does not support it automatically.
            modelBuilder.Entity<BookAuthorEntity>().HasKey(x => new { x.BookId, x.AuthorId });
            modelBuilder.Entity<BookAuthorEntity>().HasOne(x => x.Book).WithMany(x => x.Authors).HasForeignKey(x => x.BookId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<BookAuthorEntity>().HasOne(x => x.Author).WithMany(x => x.Books).HasForeignKey(x => x.AuthorId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}