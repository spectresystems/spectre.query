namespace Spectre.Query.AspNetCore.Example.Data
{
    public sealed class MovieProjection
    {
        public int MovieId { get; set; }
        public string Name { get; set; }
        public int ReleasedAt { get; set; }
        public int Rating { get; set; }
        public bool Seen { get; set; }
        public string Genre { get; set; }
    }
}
