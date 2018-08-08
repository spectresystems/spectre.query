using Shouldly;
using Spectre.Query.Tests.Data;
using Spectre.Query.Tests.Fixtures;
using Xunit;

namespace Spectre.Query.Tests
{
    public sealed class QueryProviderTests
    {
        private void Seeder(DataContext context)
        {
            context.Invoices.Add(new Invoice { InvoiceId = 1, Amount = 12, Paid = true });
            context.Invoices.Add(new Invoice { InvoiceId = 2, Amount = 24, Paid = true });
            context.Invoices.Add(new Invoice { InvoiceId = 3, Amount = 48, Paid = true, Comment = "Foo" });
            context.Invoices.Add(new Invoice { InvoiceId = 4, Amount = 96, Paid = false });
        }

        private void Configure(IQueryConfigurator<DataContext> options)
        {
            options.Configure<Invoice>(invoice =>
            {
                invoice.Map("Id", e => e.InvoiceId);
                invoice.Map("Paid", e => e.Paid);
                invoice.Map("Amount", e => e.Amount);
                invoice.Map("Comment", e => e.Comment);
            });
        }

        [Fact]
        public void Should_Return_Correct_Sql_For_Relational_Query()
        {
            using (var fixture = new QueryProviderFixture())
            {
                // Given
                fixture.Initialize(Configure);

                // When
                var result = fixture.ToSql<Invoice>("ID = 1");

                // Then
                result.ShouldBe("SELECT Invoices.* FROM Invoices WHERE Invoices.InvoiceId = 1");
            }
        }

        [Fact]
        public void Should_Return_Correct_Data_For_Relational_Query()
        {
            using (var fixture = new QueryProviderFixture(Seeder))
            {
                // Given
                fixture.Initialize(Configure);

                // When
                var result = fixture.Query<Invoice>("ID = 1");

                // Then
                result.Count.ShouldBe(1);
                result[0].As(invoice =>
                {
                    invoice.InvoiceId.ShouldBe(1);
                });
            }
        }

        [Fact]
        public void Should_Return_Correct_Sql_For_Negated_Query()
        {
            using (var fixture = new QueryProviderFixture())
            {
                // Given
                fixture.Initialize(Configure);

                // When
                var result = fixture.ToSql<Invoice>("!Paid");

                // Then
                result.ShouldBe("SELECT Invoices.* FROM Invoices WHERE NOT Invoices.Paid = 1");
            }
        }

        [Fact]
        public void Should_Return_Correct_Data_For_Negated_Query()
        {
            using (var fixture = new QueryProviderFixture(Seeder))
            {
                // Given
                fixture.Initialize(Configure);

                // When
                var result = fixture.Query<Invoice>("!Paid");

                // Then
                result.Count.ShouldBe(1);
                result[0].As(invoice =>
                {
                    invoice.InvoiceId.ShouldBe(4);
                });
            }
        }

        [Fact]
        public void Should_Return_Correct_Sql_For_Simplified_Boolean()
        {
            using (var fixture = new QueryProviderFixture())
            {
                // Given
                fixture.Initialize(Configure);

                // When
                var result = fixture.ToSql<Invoice>("Paid");

                // Then
                result.ShouldBe("SELECT Invoices.* FROM Invoices WHERE Invoices.Paid = 1");
            }
        }

        [Fact]
        public void Should_Return_Correct_Data_For_Simplified_Boolean()
        {
            using (var fixture = new QueryProviderFixture(Seeder))
            {
                // Given
                fixture.Initialize(Configure);

                // When
                var result = fixture.Query<Invoice>("Paid");

                // Then
                result.Count.ShouldBe(3);
                result[0].InvoiceId.ShouldBe(1);
                result[1].InvoiceId.ShouldBe(2);
                result[2].InvoiceId.ShouldBe(3);
            }
        }

        [Fact]
        public void Should_Return_Correct_Sql_For_Null_Comparison()
        {
            using (var fixture = new QueryProviderFixture())
            {
                // Given
                fixture.Initialize(Configure);

                // When
                var result = fixture.ToSql<Invoice>("Comment != null");

                // Then
                result.ShouldBe("SELECT Invoices.* FROM Invoices WHERE Invoices.Comment IS NOT NULL");
            }
        }

        [Fact]
        public void Should_Return_Correct_Data_For_Null_Comparison()
        {
            using (var fixture = new QueryProviderFixture(Seeder))
            {
                // Given
                fixture.Initialize(Configure);

                // When
                var result = fixture.Query<Invoice>("Comment != null");

                // Then
                result.Count.ShouldBe(1);
                result[0].InvoiceId.ShouldBe(3);
            }
        }
    }
}
