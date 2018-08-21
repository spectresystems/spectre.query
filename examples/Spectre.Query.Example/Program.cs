using System;
using Spectre.Query.Example.Data;

namespace Spectre.Query.Example
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Initializing database...");
            using (var context = MovieContext.Initialize())
            {
                // Build a query provider for the context.
                var provider = QueryProviderBuilder.Build(context, options =>
                {
                    options.Configure<Movie>(movie =>
                    {
                        movie.Map("Id", e => e.MovieId);
                        movie.Map("Genre", e => e.Genre.Name);
                        movie.Map("Title", e => e.Name);
                        movie.Map("Year", e => e.ReleasedAt);
                        movie.Map("Score", e => e.Rating);
                        movie.Map("Seen", e => e.Seen);
                    });
                });

                // Run the application.
                new Application(context, provider).Run();
            }
        }
    }
}
