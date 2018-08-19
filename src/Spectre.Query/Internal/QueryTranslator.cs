using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Spectre.Query.Internal.Expressions;
using Spectre.Query.Internal.Expressions.Ast;

namespace Spectre.Query.Internal
{
    internal sealed class QueryTranslator<TContext, TEntity> : QueryExpressionVisitor<QueryTranslatorContext, Expression>
        where TContext : DbContext
    {
        public static Expression<Func<TEntity, bool>> Translate(QueryExpression query)
        {
            if (query == null)
            {
                return _ => true;
            }

            // Translate the query expression to a LINQ expression.
            var visitor = new QueryTranslator<TContext, TEntity>();
            var context = new QueryTranslatorContext(typeof(TEntity));
            var expression = query.Accept(visitor, context);

            // Parse the expression and rewrite it.
            return Expression.Lambda<Func<TEntity, bool>>(expression, context.Parameter);
        }

        protected override Expression VisitAnd(QueryTranslatorContext context, AndExpression expression)
        {
            return Expression.AndAlso(
                expression.Left.Accept(this, context),
                expression.Right.Accept(this, context));
        }

        protected override Expression VisitConstant(QueryTranslatorContext context, Expressions.Ast.ConstantExpression expression)
        {
            return Expression.Constant(expression.Value);
        }

        protected override Expression VisitConversion(QueryTranslatorContext context, ConvertExpression expression)
        {
            return Expression.Convert(expression.Expression.Accept(this, context), expression.Type);
        }

        protected override Expression VisitNot(QueryTranslatorContext context, NotExpression expression)
        {
            return Expression.Not(expression.Expression.Accept(this, context));
        }

        protected override Expression VisitOr(QueryTranslatorContext context, OrExpression expression)
        {
            return Expression.OrElse(
                expression.Left.Accept(this, context),
                expression.Right.Accept(this, context));
        }

        protected override Expression VisitProperty(QueryTranslatorContext context, PropertyExpression expression)
        {
            var parameter = expression.EntityType != context.RootType
                ? (Expression)Expression.Convert(context.Parameter, expression.EntityType)
                : context.Parameter;

            var properties = new Queue<PropertyInfo>(expression.Properties);
            var current = Expression.MakeMemberAccess(parameter, properties.Dequeue());
            while (properties.Count > 0)
            {
                current = Expression.MakeMemberAccess(current, properties.Dequeue());
            }

            return current;
        }

        protected override Expression VisitRelational(QueryTranslatorContext context, RelationalExpression expression)
        {
            var left = expression.Left.Accept(this, context);
            var right = expression.Right.Accept(this, context);

            switch (expression.Operator)
            {
                case RelationalOperator.EqualTo:
                    return Expression.Equal(left, right);
                case RelationalOperator.NotEqualTo:
                    return Expression.NotEqual(left, right);
                case RelationalOperator.GreaterThan:
                    return Expression.GreaterThan(left, right);
                case RelationalOperator.GreaterThanOrEqualTo:
                    return Expression.GreaterThanOrEqual(left, right);
                case RelationalOperator.LessThan:
                    return Expression.LessThan(left, right);
                case RelationalOperator.LessThanOrEqualTo:
                    return Expression.LessThanOrEqual(left, right);
            }

            throw new InvalidOperationException($"Unknown operator '{expression.Operator}'.");
        }

        protected override Expression VisitScope(QueryTranslatorContext context, ScopeExpression expression)
        {
            return expression.Expression.Accept(this, context);
        }
    }
}
