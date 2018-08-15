using System;

namespace Spectre.Query.Tests.Infrastructure
{
    public static class TestQueryRunnerResultExtensions
    {
        public static void ShouldContainEntities(this TestQueryRunnerResult result, params int[] expected)
        {
            // Expected number of items?
            if (expected.Length != result.Result.Count)
            {
                throw new InvalidOperationException($"Expected {expected.Length} items but got {result.Result.Count}.");
            }

            // All expected items in collection?
            foreach (var item in expected)
            {
                if (result.Result.Find(x => x.InvoiceId == item) == null)
                {
                    throw new InvalidOperationException($"Item #{item} is not in result collection.");
                }
            }
        }
    }
}
