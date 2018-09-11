namespace Spectre.Query.AspNetCore.Example.Data
{
    public sealed class MovieGenre
    {
        public int MovieId { get; set; }
        public Movie Movie { get; set; }

        public int GenreId { get; set; }
        public Genre Genre { get; set; }
    }
}
