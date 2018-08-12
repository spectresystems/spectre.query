using Shouldly;
using Spectre.Query.Tests.Data;
using Spectre.Query.Tests.Fixtures;
using Xunit;

namespace Spectre.Query.Tests
{
    public sealed class QueryProviderTests
    {
        private void Configure(IQueryConfigurator<DataContext> options)
        {
            options.Configure<Invoice>(invoice =>
            {
                invoice.Map("Id", e => e.InvoiceId);
                invoice.Map("Paid", e => e.Paid);
                invoice.Map("Amount", e => e.Amount);
                invoice.Map("Comment", e => e.Comment);
                invoice.Map("Cancelled", e => e.Cancelled);
                invoice.Map("Discount", e => e.Discount);
            });
        }

        private void Seed(DataContext context)
        {
            context.Invoices.Add(new Invoice { InvoiceId = 1, Amount = 12, Paid = true, Cancelled = true, Discount = 5 });
            context.Invoices.Add(new Invoice { InvoiceId = 2, Amount = 24, Paid = true, Cancelled = true, Discount = 10 });
            context.Invoices.Add(new Invoice { InvoiceId = 3, Amount = 48, Paid = true, Comment = "Foo", Discount = 15 });
            context.Invoices.Add(new Invoice { InvoiceId = 4, Amount = 96, Paid = false, Discount = 20 });
            context.Invoices.Add(new Invoice { InvoiceId = 5, Amount = 128, Paid = true, Cancelled = true, Discount = null });
        }

        [Fact]
        public void Should_Return_Correct_Data_For_Equality_Comparison()
        {
            using (var fixture = new QueryProviderFixture(Seed))
            {
                // Given
                fixture.Initialize(Configure);

                // When
                var result = fixture.Query<Invoice>("ID = 1");

                // Then
                result.Count.ShouldBe(1);
                result[0].As(invoice => invoice.InvoiceId.ShouldBe(1));
            }
        }

        [Fact]
        public void Should_Return_Correct_Data_For_Less_Than_Comparison()
        {
            using (var fixture = new QueryProviderFixture(Seed))
            {
                // Given
                fixture.Initialize(Configure);

                // When
                var result = fixture.Query<Invoice>("ID < 2");

                // Then
                result.Count.ShouldBe(1);
                result[0].As(invoice => invoice.InvoiceId.ShouldBe(1));
            }
        }

        [Fact]
        public void Should_Return_Correct_Data_For_Less_Than_Or_Equals_Comparison()
        {
            using (var fixture = new QueryProviderFixture(Seed))
            {
                // Given
                fixture.Initialize(Configure);

                // When
                var result = fixture.Query<Invoice>("ID <= 2");

                // Then
                result.Count.ShouldBe(2);
                result[0].As(invoice => invoice.InvoiceId.ShouldBe(1));
                result[1].As(invoice => invoice.InvoiceId.ShouldBe(2));
            }
        }

        [Fact]
        public void Should_Return_Correct_Data_For_Greater_Than_Comparison()
        {
            using (var fixture = new QueryProviderFixture(Seed))
            {
                // Given
                fixture.Initialize(Configure);

                // When
                var result = fixture.Query<Invoice>("ID > 4");

                // Then
                result.Count.ShouldBe(1);
                result[0].As(invoice => invoice.InvoiceId.ShouldBe(5));
            }
        }

        [Fact]
        public void Should_Return_Correct_Data_For_Greater_Than_Or_Equals_Comparison()
        {
            using (var fixture = new QueryProviderFixture(Seed))
            {
                // Given
                fixture.Initialize(Configure);

                // When
                var result = fixture.Query<Invoice>("ID >= 4");

                // Then
                result.Count.ShouldBe(2);
                result[0].As(invoice => invoice.InvoiceId.ShouldBe(4));
                result[1].As(invoice => invoice.InvoiceId.ShouldBe(5));
            }
        }

        [Fact]
        public void Should_Return_Correct_Data_For_Negated_Query()
        {
            using (var fixture = new QueryProviderFixture(Seed))
            {
                // Given
                fixture.Initialize(Configure);

                // When
                var result = fixture.Query<Invoice>("!Paid");

                // Then
                result.Count.ShouldBe(1);
                result[0].As(invoice => invoice.InvoiceId.ShouldBe(4));
            }
        }

        [Fact]
        public void Should_Return_Correct_Data_For_Simplified_Boolean()
        {
            using (var fixture = new QueryProviderFixture(Seed))
            {
                // Given
                fixture.Initialize(Configure);

                // When
                var result = fixture.Query<Invoice>("Paid");

                // Then
                result.Count.ShouldBe(4);
                result[0].InvoiceId.ShouldBe(1);
                result[1].InvoiceId.ShouldBe(2);
                result[2].InvoiceId.ShouldBe(3);
                result[3].InvoiceId.ShouldBe(5);
            }
        }

        [Fact]
        public void Should_Return_Correct_Data_For_Conversion_Between_Nullable_And_Non_Nullable_Comparison()
        {
            using (var fixture = new QueryProviderFixture(Seed))
            {
                // Given
                fixture.Initialize(Configure);

                // When
                var result = fixture.Query<Invoice>("Discount = 20");

                // Then
                result.Count.ShouldBe(1);
                result[0].InvoiceId.ShouldBe(4);
            }
        }

        [Fact]
        public void Should_Return_Correct_Data_For_Null_Comparison_Against_Reference_Type()
        {
            using (var fixture = new QueryProviderFixture(Seed))
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

        [Fact]
        public void Should_Return_Correct_Data_For_Null_Comparison_Against_Nullable_Boolean()
        {
            using (var fixture = new QueryProviderFixture(Seed))
            {
                // Given
                fixture.Initialize(Configure);

                // When
                var result = fixture.Query<Invoice>("Discount = null");

                // Then
                result.Count.ShouldBe(1);
                result[0].InvoiceId.ShouldBe(5);
            }
        }
    }
}
