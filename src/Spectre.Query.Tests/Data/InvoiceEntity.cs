using System.ComponentModel.DataAnnotations;

namespace Spectre.Query.Tests.Data
{
    public class Invoice
    {
        [Key]
        public int InvoiceId { get; set; }
        public bool Paid { get; set; }
        public bool Cancelled { get; set; }
        public decimal Amount { get; set; }
        public string Comment { get; set; }
        public int? Discount { get; set; }
    }
}
