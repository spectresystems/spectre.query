using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Spectre.Query.Tests.Data
{
    public class Document
    {
        [Key]
        public int DocumentId { get; set; }
    }

    public class Invoice : Document
    {
        public bool Paid { get; set; }

        public bool Cancelled { get; set; }

        public decimal Amount { get; set; }

        public string Comment { get; set; }

        public decimal? Discount { get; set; }
    }
}
