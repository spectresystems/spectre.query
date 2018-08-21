using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Spectre.Query.Example.Data;

namespace Spectre.Query.Example
{
    public sealed class Application
    {
        private readonly MovieContext _context;
        private readonly IQueryProvider<MovieContext> _provider;

        public Application(MovieContext context, IQueryProvider<MovieContext> provider)
        {
            _context = context;
            _provider = provider;
        }

        public void Run()
        {
            Console.WriteLine();
            Console.WriteLine("Welcome to the Movie database terminal!");
            Console.WriteLine("Write EXIT to quit");
            Console.WriteLine();
            Console.WriteLine("Example query: NOT Seen AND Score > 60");
            Console.WriteLine();

            while (true)
            {
                try
                {
                    // Get input from the user.
                    string query = GetInput();
                    if (query.Equals("exit", StringComparison.OrdinalIgnoreCase))
                    {
                        break;
                    }

                    // Query the EF context for movies.
                    var movies = _provider
                        .Query<Movie>(_context, query)
                        .Include(m => m.Genre)
                        .ToList();

                    // Output the result.
                    ListResults(movies);
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("ERROR: {0}", ex.Message);
                    Console.WriteLine();
                    Console.ResetColor();
                }
            }
        }

        private string GetInput()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("ENTER QUERY: ");
            Console.ResetColor();
            return Console.ReadLine();
        }

        private void ListResults(List<Movie> movies)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("{0,-40}  {1,4}  {2,5}  {3, -10}  {4, 5}", "Title", "Year", "Score", "Genre", "Seen?");
            Console.WriteLine(new string('=', 72));
            Console.ResetColor();
            foreach (var movie in movies)
            {
                Console.WriteLine("{0,-40}  {1,4}  {2,5}  {3, -10}  {4, 5}",
                    movie.Name,
                    movie.ReleasedAt,
                    movie.Rating,
                    movie.Genre.Name,
                    movie.Seen ? "Yes" : "No");
            }
            Console.WriteLine();
        }
    }
}
