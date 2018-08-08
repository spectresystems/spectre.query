using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Query;

namespace Microsoft.AspNetCore.Builder
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseQueryProvider<TContext>(this IApplicationBuilder builder)
            where TContext : DbContext
        {
            var provider = builder.ApplicationServices.GetService<IQueryProvider<TContext>>();
            if (provider == null)
            {
                throw new InvalidOperationException($"Query provider for '{typeof(TContext).FullName}' has not been added.");
            }
        }
    }
}
