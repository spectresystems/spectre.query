using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Spectre.Query.Internal.Configuration;
using Spectre.Query.Internal.Expressions;
using Spectre.Query.Internal.Expressions.Rewriting;

namespace Spectre.Query.Internal
{
    internal static class QueryTranslator<TContext, TEntity>
        where TContext : DbContext
    {
        public static string ToSql(QueryConfiguration<TContext> configuration, string query)
        {
            var entityType = typeof(TEntity);
            if (!configuration.Mappings.TryGetValue(typeof(TEntity), out var entityConfiguration))
            {
                throw new InvalidOperationException("Entity has not been configured.");
            }

            // Parse the expression and rewrite it.
            var expression = ExpressionParser.Parse(query);
            expression = BooleanRewriter<TEntity>.Rewrite(expression);
            expression = NullRewriter<TEntity>.Rewrite(expression);

            // Generate the SQL statement.
            return GenerateSqlStatement(entityConfiguration, expression);
        }

        private static string GenerateSqlStatement(EntityConfiguration configuration, Expression expression)
        {
            // Generate SQL for the query.
            var statement = new List<string>
            {
                "SELECT",
                $"{configuration.TableName}.*",
                "FROM",
                configuration.TableName
            };

            // Got an expression?
            if (expression != null)
            {
                statement.Add("WHERE");

                var visitor = new QueryTranslatorVisitor();
                statement.Add(expression.Accept(visitor, configuration));
            }

            return string.Join(" ", statement);
        }
    }
}
