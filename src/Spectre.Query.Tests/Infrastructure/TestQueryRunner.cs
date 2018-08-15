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
        public static async Task<TestQueryRunnerResult> Execute(string query, Func<List<Invoice>> seeder, Action<IQueryConfigurator<DataContext>> options = null)
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
                    await context.Invoices.AddRangeAsync(seeder());
                    await context.SaveChangesAsync();

                    // Initialize provider.
                    var provider = QueryProviderBuilder.Build(context, options);

                    // Execute query.
                    var result = provider.Query<Invoice>(context, query).ToList();
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
            options.Configure<Invoice>(i =>
            {
                i.Map("Id", e => e.InvoiceId);
                i.Map("Paid", e => e.Paid);
                i.Map("Amount", e => e.Amount);
                i.Map("Comment", e => e.Comment);
                i.Map("Cancelled", e => e.Cancelled);
                i.Map("Discount", e => e.Discount);
            });
        }
    }
}
