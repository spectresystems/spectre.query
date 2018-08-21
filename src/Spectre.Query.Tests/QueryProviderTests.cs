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
        public static List<Document> DefaultSeeder()
        {
            var companies = new[]
            {
                new Company { CompanyId = 1, Name = "ACME Inc." },
                new Company { CompanyId = 2, Name = "Contoso" }
            };

            return new List<Document>
            {
                new Invoice { DocumentId = 1, Amount = -1.5M, Paid = true, Comment = "Ooz", Cancelled = true, Discount = 5, Company = companies[1] },
                new Invoice { DocumentId = 2, Amount = 1.5M, Paid = true, Comment = "Fooz", Cancelled = true, Discount = 10, Company = companies[1] },
                new Invoice { DocumentId = 3, Amount = 2, Paid = true, Comment = "Foo", Discount = 15, Company = companies[0] },
                new Invoice { DocumentId = 4, Amount = 3.5M, Paid = false, Comment = "Bar", Cancelled = false, Discount = 20, Company = companies[1] },
                new Invoice { DocumentId = 5, Amount = 4.5M, Paid = true, Cancelled = true, Discount = null, Company = companies[1] },
                new Document { DocumentId = 6 }
            };
        }

        public sealed class Configuration
        {
            [Fact]
            public async Task Should_Throw_If_Trying_To_Map_Two_Parameters_With_The_Same_Name()
            {
                // Given, When
                var result = await Record.ExceptionAsync(async () =>
                {
                    await TestQueryRunner.Execute("Foo = 1", DefaultSeeder, options =>
                    {
                        options.Configure<Document>(document =>
                        {
                            document.Map("Foo", e => e.DocumentId);
                            document.Map("Foo", e => e.DocumentId);
                        });
                    });
                });

                // Then
                result
                    .ShouldBeOfType<InvalidOperationException>()
                    .And().Message.ShouldBe("The property 'Foo' have been defined twice.");
            }

            [Fact]
            public async Task Should_Throw_If_Trying_To_Map_Inherited_Parameter_With_Already_Existing_Name()
            {
                // Given, When
                var result = await Record.ExceptionAsync(async () =>
                {
                    await TestQueryRunner.Execute("Foo = 1", DefaultSeeder, options =>
                    {
                        options.Configure<Document>(document =>
                        {
                            document.Map("Foo", e => e.DocumentId);
                            document.Map<Invoice>(invoice =>
                            {
                                invoice.Map("Foo", e => e.Cancelled);
                            });
                        });
                    });
                });

                // Then
                result
                    .ShouldBeOfType<InvalidOperationException>()
                    .And().Message.ShouldBe("The property 'Foo' have been defined twice.");
            }

            [Fact]
            public async Task Should_Throw_If_Trying_To_Map_Navigational_Property_Directly()
            {
                // Given, When
                var result = await Record.ExceptionAsync(async () =>
                {
                    await TestQueryRunner.Execute("Foo = 'Contoso'", DefaultSeeder, options =>
                    {
                        options.Configure<Document>(document =>
                        {
                            document.Map<Invoice>(invoice =>
                            {
                                invoice.Map("Foo", e => e.Company);
                            });
                        });
                    });
                });

                // Then
                result
                    .ShouldBeOfType<InvalidOperationException>()
                    .And().Message.ShouldBe("Cannot map the navigational property 'Company' directly.");
            }

            [Fact]
            public async Task Should_Throw_If_Trying_To_Map_Non_Entity_Type_Property()
            {
                // Given, When
                var result = await Record.ExceptionAsync(async () =>
                {
                    await TestQueryRunner.Execute("Foo = 'Contoso'", DefaultSeeder, options =>
                    {
                        options.Configure<Document>(document =>
                        {
                            document.Map<Invoice>(invoice =>
                            {
                                invoice.Map("Foo", e => e.Dummy);
                            });
                        });
                    });
                });

                // Then
                result
                    .ShouldBeOfType<InvalidOperationException>()
                    .And().Message.ShouldBe("The property 'Invoice.Dummy' is not mapped to an entity.");
            }
        }

        public sealed class Like
        {
            [Theory]
            [InlineData("Comment LIKE 'Foo%'", new[] { 2, 3 })]
            [InlineData("Comment ~ 'Foo%'", new[] { 2, 3 })]
            [InlineData("Comment LIKE 'Fo%'", new[] { 2, 3 })]
            [InlineData("Comment ~ 'Fo%'", new[] { 2, 3 })]
            [InlineData("Comment LIKE '%ooz'", new[] { 1, 2 })]
            [InlineData("Comment ~ '%ooz'", new[] { 1, 2 })]
            [InlineData("Comment LIKE '%o%'", new[] { 1, 2, 3 })]
            [InlineData("Comment ~ '%o%'", new[] { 1, 2, 3 })]
            public async Task Integer(string query, int[] expected)
            {
                // Given, When
                var result = await TestQueryRunner.Execute(query, DefaultSeeder);

                // Then
                result.ShouldContainEntities(expected);
            }
        }

        public sealed class Comparison
        {
            [Theory]
            [InlineData("ID = 1", new[] { 1 })]
            [InlineData("ID != 1", new[] { 2, 3, 4, 5, 6 })]
            [InlineData("ID < 2", new[] { 1 })]
            [InlineData("ID <= 2", new[] { 1, 2 })]
            [InlineData("ID > 5", new[] { 6 })]
            [InlineData("ID >= 5", new[] { 5, 6 })]
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
            [InlineData("Comment = null", new int[] { 5, 6 })]
            [InlineData("Comment != 'Foo'", new int[] { 1, 2, 4, 5, 6 })]
            [InlineData("Comment != null", new int[] { 1, 2, 3, 4 })]
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
            [InlineData("Discount = null", new int[] { 5, 6 })]
            public async Task Nullable_Decimal(string query, int[] expected)
            {
                // Given, When
                var result = await TestQueryRunner.Execute(query, DefaultSeeder);

                // Then
                result.ShouldContainEntities(expected);
            }

            [Theory]
            [InlineData("Company = 'ACME Inc.'", new int[] { 3 })]
            [InlineData("Company = 'Contoso'", new int[] { 1, 2, 4, 5 })]
            [InlineData("Company != 'Contoso'", new int[] { 3, 6 })]
            [InlineData("Company != 'Contoso' AND Company != NULL", new int[] { 3 })]
            [InlineData("Company = NULL", new int[] { 6 })]
            public async Task Navigational_Property(string query, int[] expected)
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
