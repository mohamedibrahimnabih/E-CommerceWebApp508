using E_Commerce.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using E_Commerce.ViewModels;

namespace E_Commerce.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }

        // ======================================================
        public DbSet<Company> Companies { get; set; }
        public DbSet<Product> Products { get; set; }

        public ApplicationDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build()
                .GetConnectionString("DefaultConnection");

            optionsBuilder.UseSqlServer(builder);
        }
        public DbSet<E_Commerce.ViewModels.ApplicationUserVM> ApplicationUserVM { get; set; } = default!;
        public DbSet<E_Commerce.ViewModels.LoginVM> LoginVM { get; set; } = default!;
    }
}
