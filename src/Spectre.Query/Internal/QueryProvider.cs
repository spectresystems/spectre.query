using System;
using System.Linq;
using System.Linq.Expressions;
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

        public Expression<Func<TEntity, bool>> Compile<TEntity>(string query)
            where TEntity : class
        {
            if (!_configuration.Mappings.TryGetValue(typeof(TEntity), out var entityConfiguration))
            {
                throw new InvalidOperationException("Entity has not been configured.");
            }

            // Parse the expression.
            var expression = QueryExpressionParser.Parse(entityConfiguration, query);
            expression = BooleanRewriter.Rewrite(expression);
            expression = ConversionRewriter.Rewrite(expression);

            // Translate the expression.
            return QueryTranslator<TContext, TEntity>.Translate(expression);
        }

        public IQueryable<TEntity> Query<TEntity>(TContext context, string query)
            where TEntity : class
        {
            if (!_configuration.Mappings.TryGetValue(typeof(TEntity), out var entityConfiguration))
            {
                throw new InvalidOperationException("Entity has not been configured.");
            }

            // Compile the query.
            var expression = Compile<TEntity>(query);

            // Execute the query.
            return entityConfiguration.IsQueryType
                ? context.Query<TEntity>().Where(expression)
                : context.Set<TEntity>().Where(expression);
        }
    }
}
