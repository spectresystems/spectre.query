using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Spectre.Query.Tests.Data;

namespace Spectre.Query.Tests.Infrastructure
{
    public static class TestQueryRunner
    {
        public static async Task<TestQueryRunnerResult> Execute(string query, Func<List<Document>> seeder, Action<IQueryConfigurator<DataContext>> options = null)
        {
            seeder = seeder ?? throw new InvalidOperationException(nameof(seeder));
            options = options ?? ConfigureDefaultQueryProvider;

            using (var connection = new SqliteConnection("DataSource=:memory:"))
            {
                connection.Open();

                var builder = new DbContextOptionsBuilder<DataContext>().UseSqlite(connection);
                using (var context = new DataContext(builder.Options))
                {
                    context.Database.EnsureCreated();

                    // Seed
                    await context.Documents.AddRangeAsync(seeder());
                    await context.SaveChangesAsync();

                    // Initialize provider.
                    var provider = QueryProviderBuilder.Build(context, options);

                    // Execute query.
                    var result = provider.Query<Document>(context, query).ToList();
                    return new TestQueryRunnerResult
                    {
                        Query = query,
                        Result = result
                    };
                }
            }
        }

        private static void ConfigureDefaultQueryProvider(IQueryConfigurator<DataContext> options)
        {
            options.Configure<Document>(document =>
            {
                document.Map("Id", e => e.DocumentId);
                document.Map<Invoice>(invoice =>
                {
                    invoice.Map("Company", e => e.Company.Name);
                    invoice.Map("Paid", e => e.Paid);
                    invoice.Map("Amount", e => e.Amount);
                    invoice.Map("Comment", e => e.Comment);
                    invoice.Map("Cancelled", e => e.Cancelled);
                    invoice.Map("Discount", e => e.Discount);
                });
            });
        }
    }
}
