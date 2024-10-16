using BulkyWeb.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace BulkyWeb.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {
            
        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<Borrow> Borrows { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Action", DisplayOrder = 1, NumerOfBorrow = 0 },
                new Category { Id = 2, Name = "SciFi", DisplayOrder = 2, NumerOfBorrow = 0 },
                new Category { Id = 3, Name = "History", DisplayOrder = 3, NumerOfBorrow = 2  }
                );

            modelBuilder.Entity<Account>().HasData(
                new Account { Id = 1, Email = "phong@gmai.com", Password = "123456", Position = "Quản lý" },
                new Account { Id = 2, Email = "phong@gmail.com", Password = "123456", Position = "Nhân viên" }
                );

            modelBuilder.Entity<Borrow>().HasData(
                new Borrow { Id = 1, Email = "phong@gmai.com", NameBook = "History", NumerBorrow = 1, TimeBorrow = new DateTime(2024, 12, 11) }
                );
        }
    }
}
