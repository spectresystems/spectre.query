using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Spectre.Query.AspNetCore.Example.Data;
using Spectre.Query.AspNetCore.Example.Models;

namespace Spectre.Query.AspNetCore.Example.Controllers
{
    public class HomeController : Controller
    {
        private readonly IQueryProviderSession<MovieContext> _provider;

        public HomeController(IQueryProviderSession<MovieContext> provider)
        {
            _provider = provider;
        }

        [HttpGet]
        public IActionResult Index(string query = null)
        {
            var movies = _provider
                .Query<Movie>(query)
                .OrderByDescending(x => x.Rating);

            return View(new SearchResultModel<Movie>
            {
                Movies = movies.ToList(),
                Query = query
            });
        }

        [HttpPost]
        public IActionResult Search(SearchResultModel<Movie> model)
        {
            return RedirectToAction(nameof(Index), new { query = model.Query });
        }

        [HttpGet]
        public IActionResult Projection(string query = null)
        {
            var movies = _provider
                .Query<MovieProjection>(query)
                .OrderByDescending(x => x.Rating);

            return View(new SearchResultModel<MovieProjection>
            {
                Movies = movies.ToList(),
                Query = query
            });
        }

        [HttpPost]
        public IActionResult SearchProjection(SearchResultModel<Movie> model)
        {
            return RedirectToAction(nameof(Projection), new { query = model.Query });
        }
    }
}
