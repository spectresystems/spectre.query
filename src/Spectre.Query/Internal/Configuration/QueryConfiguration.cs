using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Spectre.Query.Internal.Configuration
{
    internal sealed class QueryConfiguration<TContext>
        where TContext : DbContext
    {
        private readonly Dictionary<Type, EntityConfiguration> _configurations;

        public IReadOnlyDictionary<Type, EntityConfiguration> Mappings => _configurations;

        public QueryConfiguration()
        {
            _configurations = new Dictionary<Type, EntityConfiguration>();
        }

        public void Add<TEntity>(EntityConfigurator<TContext, TEntity> configurator)
        {
            _configurations.Add(typeof(TEntity), configurator.Configuration);
        }
    }
}
