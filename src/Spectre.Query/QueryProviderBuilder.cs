using System;
using Microsoft.EntityFrameworkCore;
using Spectre.Query.Internal;
using Spectre.Query.Internal.Configuration;

namespace Spectre.Query
{
    public static class QueryProviderBuilder
    {
        public static IQueryProvider<TContext> Build<TContext>(TContext context, Action<IQueryConfigurator<TContext>> options)
            where TContext : DbContext
        {
            // Create a new configuration.
            var configuration = new QueryConfiguration<TContext>();

            // Configure the configuration.
            var configurator = new QueryConfigurator<TContext>(context, configuration);
            options(configurator);

            // Create the query provider with the configuration.
            return new QueryProvider<TContext>(configuration);
        }
    }
}
