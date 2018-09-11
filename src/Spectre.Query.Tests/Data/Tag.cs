using System.ComponentModel.DataAnnotations;

namespace Spectre.Query.Tests.Data
{
    public class Tag
    {
        [Key]
        public int TagId { get; set; }

        public string Name { get; set; }
        public int Weight { get; set; }
    }
}
