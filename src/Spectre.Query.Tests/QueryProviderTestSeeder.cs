using System.Collections.Generic;
using Spectre.Query.Tests.Data;

namespace Spectre.Query.Tests
{
    public static class QueryProviderTestSeeder
    {
        public static void DefaultSeeder(DataContext context)
        {
            var companies = new[]
            {
                new Company { CompanyId = 1, Name = "ACME Inc." },
                new Company { CompanyId = 2, Name = "Contoso" }
            };

            var bank = new Tag { Name = "Bank", Weight = 1 };
            var insurance = new Tag { Name = "Insurance", Weight = 5 };
            var tax = new Tag { Name = "Tax", Weight = 10 };

            var docs = new List<Document>
            {
                new Invoice { DocumentId = 1, Amount = -1.5M, Paid = true, Comment = "Ooz", Cancelled = true, Discount = 5, Company = companies[1] },
                new Invoice { DocumentId = 2, Amount = 1.5M, Paid = true, Comment = "Fooz", Cancelled = true, Discount = 10, Company = companies[1] },
                new Invoice { DocumentId = 3, Amount = 2, Paid = true, Comment = "Foo", Discount = 15, Company = companies[0] },
                new Invoice { DocumentId = 4, Amount = 3.5M, Paid = false, Comment = "Bar", Cancelled = false, Discount = 20, Company = companies[1] },
                new Invoice { DocumentId = 5, Amount = 4.5M, Paid = true, Cancelled = true, Discount = null, Company = companies[1] },
                new Document { DocumentId = 6 }
            };

            context.AddRange(companies);
            context.AddRange(new[] { bank, insurance, tax });
            context.AddRange(docs);

            // Connect tags to documents
            context.ConnectTags(
                (docs[0], new[] { bank }),
                (docs[1], new[] { bank, insurance }),
                (docs[2], new[] { bank, insurance, tax }),
                (docs[3], new[] { insurance, tax }),
                (docs[4], new[] { tax }));

            context.SaveChanges();
        }

        public static void ConnectTags(this DataContext context, params (Document document, Tag[] tags)[] items)
        {
            foreach (var (document, tags) in items)
            {
                foreach (var tag in tags)
                {
                    context.Add(new DocumentTag { Document = document, Tag = tag });
                }
            }
        }
    }
}
