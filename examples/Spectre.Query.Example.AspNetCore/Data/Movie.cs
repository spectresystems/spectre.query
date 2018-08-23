using System.Collections.Generic;

namespace Spectre.Query.AspNetCore.Example.Data
{
    public sealed class Movie
    {
        public int MovieId { get; set; }
        public string Name { get; set; }
        public int ReleasedAt { get; set; }
        public int Rating { get; set; }
        public bool Seen { get; set; }

        public List<MovieGenre> Genres { get; set; }
    }
}
