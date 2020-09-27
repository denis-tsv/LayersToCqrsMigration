using Domain;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.MsSql
{
    public class AppDbContext : DbContext, IDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Product> Products { get; set; }
        public new DbSet<TEntity> Set<TEntity>() where TEntity : Entity => base.Set<TEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>().HasData(
                new Order { Id = 1, UserEmail = "test@test.test" },
                new Order { Id = 2, UserEmail = "other_email" }
            );
        }
    }
}
