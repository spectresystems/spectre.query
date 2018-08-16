using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shouldly;
using Spectre.Query.Tests.Data;
using Spectre.Query.Tests.Infrastructure;
using Xunit;

namespace Spectre.Query.Tests
{
    public static class QueryProviderTests
    {
        public static List<Invoice> DefaultSeeder()
        {
            return new List<Invoice>
            {
                new Invoice { InvoiceId = 1, Amount = -1.5M, Paid = true, Cancelled = true, Discount = 5 },
                new Invoice { InvoiceId = 2, Amount = 1.5M, Paid = true, Cancelled = true, Discount = 10 },
                new Invoice { InvoiceId = 3, Amount = 2, Paid = true, Comment = "Foo", Discount = 15 },
                new Invoice { InvoiceId = 4, Amount = 3.5M, Paid = false, Cancelled = false, Discount = 20 },
                new Invoice { InvoiceId = 5, Amount = 4.5M, Paid = true, Cancelled = true, Discount = null }
            };
        }

        public sealed class Comparison
        {
            [Theory]
            [InlineData("ID = 1", new[] { 1 })]
            [InlineData("ID != 1", new[] { 2, 3, 4, 5 })]
            [InlineData("ID < 2", new[] { 1 })]
            [InlineData("ID <= 2", new[] { 1, 2 })]
            [InlineData("ID > 4", new[] { 5 })]
            [InlineData("ID >= 4", new[] { 4, 5 })]
            public async Task Integer(string query, int[] expected)
            {
                // Given, When
                var result = await TestQueryRunner.Execute(query, DefaultSeeder);

                // Then
                result.ShouldContainEntities(expected);
            }

            [Theory]
            [InlineData("Paid", new int[] { 1, 2, 3, 5 })]
            [InlineData("Paid = true", new int[] { 1, 2, 3, 5 })]
            [InlineData("!Paid", new int[] { 4 })]
            [InlineData("Paid = false", new int[] { 4 })]
            public async Task Boolean(string query, int[] expected)
            {
                // Given, When
                var result = await TestQueryRunner.Execute(query, DefaultSeeder);

                // Then
                result.ShouldContainEntities(expected);
            }

            [Theory]
            [InlineData("Comment = 'Foo'", new int[] { 3 })]
            [InlineData("Comment = null", new int[] { 1, 2, 4, 5 })]
            [InlineData("Comment != 'Foo'", new int[] { 1, 2, 4, 5 })]
            [InlineData("Comment != null", new int[] { 3 })]
            public async Task String(string query, int[] expected)
            {
                // Given, When
                var result = await TestQueryRunner.Execute(query, DefaultSeeder);

                // Then
                result.ShouldContainEntities(expected);
            }

            [Theory]
            [InlineData("Amount = -1.5", new int[] { 1 })]
            [InlineData("Amount = 1.5", new int[] { 2 })]
            [InlineData("Amount = 2", new int[] { 3 })]
            [InlineData("Amount = 2.0", new int[] { 3 })]
            [InlineData("Amount < 1.5", new int[] { 1 })]
            [InlineData("Amount <= 1.5", new int[] { 1, 2 })]
            [InlineData("Amount > 3.5", new int[] { 5 })]
            [InlineData("Amount >= 3.5", new int[] { 4, 5 })]
            public async Task Decimal(string query, int[] expected)
            {
                // Given, When
                var result = await TestQueryRunner.Execute(query, DefaultSeeder);

                // Then
                result.ShouldContainEntities(expected);
            }

            [Theory]
            [InlineData("Discount = 20", new int[] { 4 })]
            [InlineData("Discount = 20.0", new int[] { 4 })]
            [InlineData("Discount = null", new int[] { 5 })]
            public async Task Nullable_Decimal(string query, int[] expected)
            {
                // Given, When
                var result = await TestQueryRunner.Execute(query, DefaultSeeder);

                // Then
                result.ShouldContainEntities(expected);
            }
        }

        public sealed class Tokenization
        {
            [Theory]
            [InlineData("Amount = 01")]
            [InlineData("Amount = -01")]
            public async Task Should_Throw_If_Integer_Has_Invalid_Format(string query)
            {
                // Given, When
                var result = await Record.ExceptionAsync(
                    () => TestQueryRunner.Execute(query, DefaultSeeder));

                // Then
                result
                    .ShouldBeOfType<InvalidOperationException>()
                    .And().Message.ShouldBe("Invalid number format.");
            }

            [Theory]
            [InlineData("Amount = 00.1")]
            [InlineData("Amount = -00.1")]
            [InlineData("Amount = -0.1z")]
            [InlineData("Amount = -0.1.1")]
            public async Task Should_Throw_If_Decimal_Has_Invalid_Format(string query)
            {
                // Given, When
                var result = await Record.ExceptionAsync(
                    () => TestQueryRunner.Execute(query, DefaultSeeder));

                // Then
                result
                    .ShouldBeOfType<InvalidOperationException>()
                    .And().Message.ShouldBe("Invalid number format.");
            }
        }
    }
}
