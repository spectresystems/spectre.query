using System.Collections.Generic;
using Spectre.Query.AspNetCore.Example.Data;

namespace Spectre.Query.Example.AspNetCore.Data
{
    public static class MovieContextSeeder
    {
        public static void Seed(MovieContext context)
        {
            var drama = new Genre { Name = "Drama" };
            var crime = new Genre { Name = "Crime" };
            var scifi = new Genre { Name = "Sci-Fi" };
            var action = new Genre { Name = "Action" };
            var comedy = new Genre { Name = "Comedy" };
            var thriller = new Genre { Name = "Thriller" };
            var family = new Genre { Name = "Family" };
            var fantasy = new Genre { Name = "Fantasy" };
            var adventure = new Genre { Name = "Adventure" };
            var horror = new Genre { Name = "Horror" };
            var mystery = new Genre { Name = "Mystery" };
            var biography = new Genre { Name = "Biography" };
            var romance = new Genre { Name = "Romance" };

            var movies = new List<Movie>
            {
                new Movie { Name = "The Shawshank Redemption", Rating = 93, ReleasedAt = 1994, Seen = true },
                new Movie { Name = "The Godfather", Rating = 92, ReleasedAt = 1972, Seen = true },
                new Movie { Name = "It's a Wonderful Life", Rating = 86, ReleasedAt = 1946, Seen = false },
                new Movie { Name = "Back to the Future", Rating = 85, ReleasedAt = 1985, Seen = true },
                new Movie { Name = "Psycho", Rating = 85, ReleasedAt = 1960, Seen = true },
                new Movie { Name = "The Fast and the Furious: Tokyo Drift", Rating = 60, ReleasedAt = 2006, Seen = false },
                new Movie { Name = "Spider-Man 3", Rating = 62, ReleasedAt = 2007, Seen = false },
                new Movie { Name = "Interstellar", Rating = 86, ReleasedAt = 2014, Seen = true },
                new Movie { Name = "The Disaster Artist", Rating = 75, ReleasedAt = 2017, Seen = true },
                new Movie { Name = "The Room", Rating = 36, ReleasedAt = 2003, Seen = true },
                new Movie { Name = "Zoolander", Rating = 66, ReleasedAt = 2001, Seen = true },
                new Movie { Name = "The Big Sick", Rating = 76, ReleasedAt = 2017, Seen = false },
                new Movie { Name = "Moonlight", Rating = 74, ReleasedAt = 2016, Seen = true },
                new Movie { Name = "Get Out", Rating = 77, ReleasedAt = 2017, Seen = false },
                new Movie { Name = "Perfetti Sconosciuti", Rating = 78, ReleasedAt = 2016, Seen = true },
                new Movie { Name = "Jurassic Park", Rating = 81, ReleasedAt = 1993, Seen = true },
                new Movie { Name = "Little Miss Sunshine", Rating = 78, ReleasedAt = 2006, Seen = true },
                new Movie { Name = "The Usual Suspects", Rating = 86, ReleasedAt = 1995, Seen = true },
                new Movie { Name = "Gremlings", Rating = 72, ReleasedAt = 1984, Seen = true },
                new Movie { Name = "Stuart Little", Rating = 59, ReleasedAt = 1999, Seen = false },
                new Movie { Name = "Cars 2", Rating = 62, ReleasedAt = 2011, Seen = false },
                new Movie { Name = "Zombieland", Rating = 77, ReleasedAt = 2009, Seen = true },
                new Movie { Name = "The Pursuit of Happyness", Rating = 80, ReleasedAt = 2006, Seen = false },
                new Movie { Name = "Fargo", Rating = 81, ReleasedAt = 1996, Seen = true }
            };

            context.Movies.AddRange(movies);
            context.Genres.AddRange(
                drama, crime, scifi, action, comedy, thriller, family,
                fantasy, adventure, horror, mystery, biography, romance);

            context.ConnectTags(
                (movies[0], new[] { drama }),
                (movies[1], new[] { crime }),
                (movies[2], new[] { drama, family, fantasy }),
                (movies[3], new[] { adventure, drama, scifi }),
                (movies[4], new[] { horror, mystery, thriller }),
                (movies[5], new[] { action, crime, thriller }),
                (movies[6], new[] { action, adventure, scifi }),
                (movies[7], new[] { adventure, drama, scifi }),
                (movies[8], new[] { biography, comedy, drama }),
                (movies[9], new[] { drama }),
                (movies[10], new[] { comedy }),
                (movies[11], new[] { comedy, drama, romance }),
                (movies[12], new[] { drama }),
                (movies[13], new[] { horror, mystery, thriller }),
                (movies[14], new[] { comedy, drama }),
                (movies[15], new[] { adventure, scifi, thriller }),
                (movies[16], new[] { comedy, drama }),
                (movies[17], new[] { crime, mystery, thriller }),
                (movies[18], new[] { comedy, fantasy, horror }),
                (movies[19], new[] { adventure, comedy, family }),
                (movies[20], new[] { family, adventure, comedy }),
                (movies[21], new[] { adventure, comedy, horror }),
                (movies[22], new[] { biography, drama }),
                (movies[23], new[] { crime, drama, thriller }));

            context.SaveChanges();
        }

        public static void ConnectTags(this MovieContext context, params (Movie document, Genre[] genres)[] items)
        {
            foreach (var (movie, genres) in items)
            {
                foreach (var genre in genres)
                {
                    context.Add(new MovieGenre { Movie = movie, Genre = genre });
                }
            }
        }
    }
}
