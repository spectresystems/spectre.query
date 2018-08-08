using System;
using Microsoft.EntityFrameworkCore;

namespace Spectre.Query.Internal.Configuration
{
    internal sealed class QueryConfigurator<TContext> : IQueryConfigurator<TContext>
        where TContext : DbContext
    {
        private readonly TContext _context;
        private readonly QueryConfiguration<TContext> _configuration;

        public QueryConfigurator(TContext context, QueryConfiguration<TContext> configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public void Configure<TEntity>(Action<IEntityConfigurator<TEntity>> options)
        {
            if (_configuration.Mappings.ContainsKey(typeof(TEntity)))
            {
                throw new InvalidOperationException("Entity has already been configured.");
            }

            var entityType = _context.Model.FindEntityType(typeof(TEntity));
            if (entityType == null)
            {
                throw new InvalidOperationException("Entity is not part of DbContext.");
            }

            // Execute the entity configuration.
            var configurator = new EntityConfigurator<TContext, TEntity>(_context, entityType);
            options(configurator);

            // Add the entity configuration to the configuration.
            _configuration.Add(configurator);
        }
    }
}
