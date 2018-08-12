using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Spectre.Query.Internal.Configuration;
using Spectre.Query.Internal.Expressions;
using Spectre.Query.Internal.Expressions.Rewriting;

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

        public IQueryable<TEntity> Query<TEntity>(TContext context, string query)
            where TEntity : class
        {
            if (!_configuration.Mappings.TryGetValue(typeof(TEntity), out var entityConfiguration))
            {
                throw new InvalidOperationException("Entity has not been configured.");
            }

            // Parse the expression.
            var expression = QueryExpressionParser.Parse(entityConfiguration, query);
            expression = BooleanRewriter.Rewrite(expression);
            expression = NullableRewriter.Rewrite(expression);

            // Translate the expression.
            var result = QueryTranslator<TContext, TEntity>.Translate(expression);
            return entityConfiguration.IsQueryType
                ? context.Query<TEntity>().Where(result)
                : context.Set<TEntity>().Where(result);
        }
    }
}
