﻿using System;
using Microsoft.EntityFrameworkCore;
using Spectre.Query;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void AddQueryProvider<TContext>(this IServiceCollection services, Action<IQueryConfigurator<TContext>> configurator)
            where TContext : DbContext
        {
            // Register the query provider.
            services.AddSingleton(f =>
            {
                var factory = f.GetRequiredService<IServiceScopeFactory>();
                using (var scope = factory.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<TContext>();
                    return QueryProviderBuilder.Build(context, configurator);
                }
            });
        }
    }
}
