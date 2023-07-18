using FastReportGenerator.Models;
using Microsoft.EntityFrameworkCore;

namespace FastReportGenerator.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
