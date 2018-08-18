using Microsoft.EntityFrameworkCore;

namespace Spectre.Query.Tests.Data
{
    public sealed class DataContext : DbContext
    {
        public DbSet<Document> Documents { get; set; }

        public DataContext()
        {
        }

        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Document>()
                .HasDiscriminator<string>("DocumentType")
                .HasValue<Document>("Document")
                .HasValue<Invoice>("Invoice");
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
