using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spectre.Query.AspNetCore.Example.Data;
using Spectre.Query.AspNetCore.Example.Models;

namespace Spectre.Query.AspNetCore.Example.Controllers
{
    public class HomeController : Controller
    {
        private readonly MovieContext _context;
        private readonly IQueryProvider<MovieContext> _provider;

        public HomeController(MovieContext context, IQueryProvider<MovieContext> provider)
        {
            _context = context;
            _provider = provider;
        }

        [HttpGet]
        public IActionResult Index(string query = null)
        {
            var movies = _provider.Query<Movie>(_context, query)
                .Include(m => m.Genres).ThenInclude(g => g.Genre)
                .OrderByDescending(movie => movie.Rating)
                .ToList();

            return View(new SearchResultModel<Movie>
            {
                Movies = movies,
                Query = query
            });
        }

        [HttpPost]
        public IActionResult Search(SearchResultModel<Movie> model)
        {
            return RedirectToAction(nameof(Index), new { query = model.Query });
        }
    }
}
