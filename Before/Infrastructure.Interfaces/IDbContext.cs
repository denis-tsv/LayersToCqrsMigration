using System.Threading;
using System.Threading.Tasks;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Interfaces
{
    public interface IDbContext
    {
        DbSet<Order> Orders { get; }
        DbSet<Product> Products { get; }
        DbSet<TEntity> Set<TEntity>() where TEntity : Entity;

        Task<int> SaveChangesAsync(CancellationToken token = default);
    }
}