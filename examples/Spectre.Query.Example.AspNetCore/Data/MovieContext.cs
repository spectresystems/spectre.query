using Microsoft.EntityFrameworkCore;

namespace Spectre.Query.AspNetCore.Example.Data
{
    public sealed class MovieContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }

        public MovieContext(DbContextOptions<MovieContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MovieGenre>()
                .HasKey(t => new { t.MovieId, t.GenreId });
        }
    }
}
