using Microsoft.EntityFrameworkCore;
using retail_management.Models;

namespace retail_management.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<TransactionDetail> TransactionDetails { get; set; }

        public DbSet<Invoice> Invoices { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

        }
    }
}
