using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Spectre.Query.Internal.Configuration;

namespace Spectre.Query.Internal
{
    internal sealed class QueryProvider<TContext> : IQueryProvider<TContext>
        where TContext : DbContext
    {
        private readonly QueryConfiguration<TContext> _configuration;

        public QueryProvider(QueryConfiguration<TContext> configuration)
        {
            _configuration = configuration;
        }

        public string ToSql<TEntity>(string query)
            where TEntity : class
        {
            return QueryTranslator<TContext, TEntity>.ToSql(_configuration, query);
        }

        public IQueryable<TEntity> Query<TEntity>(TContext context, string query)
            where TEntity : class
        {
            if (!_configuration.Mappings.TryGetValue(typeof(TEntity), out var entityConfiguration))
            {
                throw new InvalidOperationException("Entity has not been configured.");
            }

            var sql = ToSql<TEntity>(query);

            return entityConfiguration.IsQueryType
                ? context.Query<TEntity>().FromSql(sql)
                : context.Set<TEntity>().FromSql(sql);
        }
    }
}
