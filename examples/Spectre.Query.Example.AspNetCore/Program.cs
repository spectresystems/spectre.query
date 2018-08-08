using System.Linq;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Query.AspNetCore.Example.Data;

namespace Spectre.Query.AspNetCore.Example
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<MovieContext>();

                context.Database.Migrate();

                if (context.Movies.Count() == 0)
                {
                    var drama = context.Genres.Add(new Genre { Name = "Drama" });
                    var crime = context.Genres.Add(new Genre { Name = "Crime" });
                    var scifi = context.Genres.Add(new Genre { Name = "Sci-Fi" });
                    var action = context.Genres.Add(new Genre { Name = "Action" });
                    var comedy = context.Genres.Add(new Genre { Name = "Comedy" });
                    var thriller = context.Genres.Add(new Genre { Name = "Thriller" });
                    var family = context.Genres.Add(new Genre { Name = "Family" });
                    var adventure = context.Genres.Add(new Genre { Name = "Adventure" });

                    context.Movies.Add(new Movie { Name = "The Shawshank Redemption", Rating = 93, ReleasedAt = 1994, Seen = true, Genre = drama.Entity });
                    context.Movies.Add(new Movie { Name = "The Godfather", Rating = 92, ReleasedAt = 1972, Seen = true, Genre = drama.Entity });
                    context.Movies.Add(new Movie { Name = "It's a Wonderful Life", Rating = 86, ReleasedAt = 1946, Seen = false, Genre = drama.Entity });
                    context.Movies.Add(new Movie { Name = "Back to the Future", Rating = 85, ReleasedAt = 1985, Seen = true, Genre = scifi.Entity });
                    context.Movies.Add(new Movie { Name = "Psycho", Rating = 85, ReleasedAt = 1960, Seen = true, Genre = thriller.Entity });
                    context.Movies.Add(new Movie { Name = "The Fast and the Furious: Tokyo Drift", Rating = 60, ReleasedAt = 2006, Seen = false, Genre = action.Entity });
                    context.Movies.Add(new Movie { Name = "Spider-Man 3", Rating = 62, ReleasedAt = 2007, Seen = false, Genre = action.Entity });
                    context.Movies.Add(new Movie { Name = "Interstellar", Rating = 86, ReleasedAt = 2014, Seen = true, Genre = scifi.Entity });
                    context.Movies.Add(new Movie { Name = "The Disaster Artist", Rating = 75, ReleasedAt = 2017, Seen = true, Genre = drama.Entity });
                    context.Movies.Add(new Movie { Name = "The Room", Rating = 36, ReleasedAt = 2003, Seen = true, Genre = drama.Entity });
                    context.Movies.Add(new Movie { Name = "Zoolander", Rating = 66, ReleasedAt = 2001, Seen = true, Genre = comedy.Entity });
                    context.Movies.Add(new Movie { Name = "The Big Sick", Rating = 76, ReleasedAt = 2017, Seen = false, Genre = drama.Entity });
                    context.Movies.Add(new Movie { Name = "Moonlight", Rating = 74, ReleasedAt = 2016, Seen = true, Genre = drama.Entity });
                    context.Movies.Add(new Movie { Name = "Get Out", Rating = 77, ReleasedAt = 2017, Seen = false, Genre = thriller.Entity });
                    context.Movies.Add(new Movie { Name = "Perfetti Sconosciuti", Rating = 78, ReleasedAt = 2016, Seen = true, Genre = drama.Entity });
                    context.Movies.Add(new Movie { Name = "Jurassic Park", Rating = 81, ReleasedAt = 1993, Seen = true, Genre = adventure.Entity });
                    context.Movies.Add(new Movie { Name = "Little Miss Sunshine", Rating = 78, ReleasedAt = 2006, Seen = true, Genre = drama.Entity });
                    context.Movies.Add(new Movie { Name = "The Usual Suspects", Rating = 86, ReleasedAt = 1995, Seen = true, Genre = crime.Entity });
                    context.Movies.Add(new Movie { Name = "Gremlings", Rating = 72, ReleasedAt = 1984, Seen = true, Genre = comedy.Entity });
                    context.Movies.Add(new Movie { Name = "Stuart Little", Rating = 59, ReleasedAt = 1999, Seen = false, Genre = family.Entity });
                    context.Movies.Add(new Movie { Name = "Cars 2", Rating = 62, ReleasedAt = 2011, Seen = false, Genre = family.Entity });
                    context.Movies.Add(new Movie { Name = "Zombieland", Rating = 77, ReleasedAt = 2009, Seen = true, Genre = thriller.Entity });
                    context.Movies.Add(new Movie { Name = "The Pursuit of Happyness", Rating = 80, ReleasedAt = 2006, Seen = false, Genre = drama.Entity });
                    context.Movies.Add(new Movie { Name = "Fargo", Rating = 81, ReleasedAt = 1996, Seen = true, Genre = drama.Entity });

                    // Create a view.
                    context.Database.ExecuteSqlCommand(
                        @"CREATE VIEW View_Movies AS 
                            SELECT Movies.MovieId, Movies.Name, Movies.ReleasedAt, Movies.Rating, Movies.Seen, Movies.GenreId, Genres.Name AS GenreName
                            FROM Movies INNER JOIN Genres ON Movies.GenreId = Genres.GenreId");

                    context.SaveChanges();
                }
            }

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
