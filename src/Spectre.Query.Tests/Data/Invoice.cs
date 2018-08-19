using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Spectre.Query.Tests.Data
{
    public class Invoice : Document
    {
        [NotMapped]
        public string Dummy { get; set; }

        public Company Company { get; set; }

        public bool Paid { get; set; }

        public bool Cancelled { get; set; }

        public decimal Amount { get; set; }

        public string Comment { get; set; }

        public decimal? Discount { get; set; }
    }
}
