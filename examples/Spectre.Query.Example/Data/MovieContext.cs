using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Spectre.Query.Example.Data
{
    public sealed class MovieContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=MovieDatabaseConsole;Trusted_Connection=True;");
        }

        public static MovieContext Initialize()
        {
            var context = new MovieContext();
            context.Database.Migrate();

            if (context.Movies.Count() == 0)
            {
                context.Movies.Add(new Movie { Name = "The Shawshank Redemption", Rating = 93, ReleasedAt = 1994, Seen = true });
                context.Movies.Add(new Movie { Name = "The Godfather", Rating = 92, ReleasedAt = 1972, Seen = true });
                context.Movies.Add(new Movie { Name = "It's a Wonderful Life", Rating = 86, ReleasedAt = 1946, Seen = false });
                context.Movies.Add(new Movie { Name = "Back to the Future", Rating = 85, ReleasedAt = 1985, Seen = true });
                context.Movies.Add(new Movie { Name = "Psycho", Rating = 85, ReleasedAt = 1960, Seen = true });
                context.Movies.Add(new Movie { Name = "The Fast and the Furious: Tokyo Drift", Rating = 60, ReleasedAt = 2006, Seen = false });
                context.Movies.Add(new Movie { Name = "Spider-Man 3", Rating = 62, ReleasedAt = 2007, Seen = false });
                context.Movies.Add(new Movie { Name = "Interstellar", Rating = 86, ReleasedAt = 2014, Seen = true });
                context.Movies.Add(new Movie { Name = "The Disaster Artist", Rating = 75, ReleasedAt = 2017, Seen = true });
                context.Movies.Add(new Movie { Name = "The Room", Rating = 36, ReleasedAt = 2003, Seen = true });
                context.Movies.Add(new Movie { Name = "Zoolander", Rating = 66, ReleasedAt = 2001, Seen = true });
                context.Movies.Add(new Movie { Name = "The Big Sick", Rating = 76, ReleasedAt = 2017, Seen = false });
                context.Movies.Add(new Movie { Name = "Moonlight", Rating = 74, ReleasedAt = 2016, Seen = true });
                context.Movies.Add(new Movie { Name = "Get Out", Rating = 77, ReleasedAt = 2017, Seen = false });
                context.Movies.Add(new Movie { Name = "Perfetti Sconosciuti", Rating = 78, ReleasedAt = 2016, Seen = true });
                context.Movies.Add(new Movie { Name = "Jurassic Park", Rating = 81, ReleasedAt = 1993, Seen = true });
                context.Movies.Add(new Movie { Name = "Little Miss Sunshine", Rating = 78, ReleasedAt = 2006, Seen = true });
                context.Movies.Add(new Movie { Name = "The Usual Suspects", Rating = 86, ReleasedAt = 1995, Seen = true });
                context.Movies.Add(new Movie { Name = "Gremlings", Rating = 72, ReleasedAt = 1984, Seen = true });
                context.Movies.Add(new Movie { Name = "Stuart Little", Rating = 59, ReleasedAt = 1999, Seen = false });
                context.Movies.Add(new Movie { Name = "Cars 2", Rating = 62, ReleasedAt = 2011, Seen = false });
                context.Movies.Add(new Movie { Name = "Zombieland", Rating = 77, ReleasedAt = 2009, Seen = true });
                context.Movies.Add(new Movie { Name = "The Pursuit of Happyness", Rating = 80, ReleasedAt = 2006, Seen = false });
                context.Movies.Add(new Movie { Name = "Fargo", Rating = 81, ReleasedAt = 1996, Seen = true });

                context.SaveChanges();
            }

            return context;
        }
    }
}
