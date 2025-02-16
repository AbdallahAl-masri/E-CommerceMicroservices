using Microsoft.EntityFrameworkCore;
using Order.Domain.Entites;

namespace Order.Infrastructure.Data
{
    public class OrderDbContext(DbContextOptions<OrderDbContext> options) : DbContext(options)
    {
        public DbSet<Orders> Orders { get; set; }
    }
}
