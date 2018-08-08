using Microsoft.EntityFrameworkCore;

namespace Spectre.Query.Tests.Data
{
    public sealed class DataContext : DbContext
    {
        public DbSet<Invoice> Invoices { get; set; }

        public DataContext()
        {
        }

        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Invoice>()
                .ToTable("Invoices");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=QueryDatabase;Trusted_Connection=True;ConnectRetryCount=0");
            }
        }
    }
}
