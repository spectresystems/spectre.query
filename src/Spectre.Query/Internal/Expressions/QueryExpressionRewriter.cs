using System;
using Spectre.Query.Internal.Expressions.Ast;

namespace Spectre.Query.Internal.Expressions
{
    internal abstract class QueryExpressionRewriter<TContext> : QueryExpressionVisitor<TContext, QueryExpression>
    {
        protected override QueryExpression VisitAnd(TContext context, AndExpression expression)
        {
            return VisitBinary(context, expression, (left, right) =>
                new AndExpression(left, right));
        }

        protected override QueryExpression VisitConversion(TContext context, ConvertExpression expression)
        {
            return VisitUnary(context, expression, child =>
                new ConvertExpression(child, expression.Type));
        }

        protected override QueryExpression VisitConstant(TContext context, ConstantExpression expression)
        {
            return expression;
        }

        protected override QueryExpression VisitCollection(TContext context, CollectionExpression expression)
        {
            return expression;
        }

        protected override QueryExpression VisitLike(TContext context, LikeExpression expression)
        {
            return VisitBinary(context, expression, (left, right) =>
                new LikeExpression(left, right));
        }

        protected override QueryExpression VisitNot(TContext context, NotExpression expression)
        {
            return VisitUnary(context, expression, child =>
                new NotExpression(child));
        }

        protected override QueryExpression VisitOr(TContext context, OrExpression expression)
        {
            return VisitBinary(context, expression, (left, right) =>
                new OrExpression(left, right));
        }

        protected override QueryExpression VisitRelation(TContext context, RelationalExpression expression)
        {
            return VisitBinary(context, expression, (left, right) =>
                new RelationalExpression(left, right, expression.Operator));
        }

        protected override QueryExpression VisitScope(TContext context, ScopeExpression expression)
        {
            return VisitUnary(context, expression, child =>
                new ScopeExpression(child));
        }

        protected override QueryExpression VisitProperty(TContext context, PropertyExpression expression)
        {
            return expression;
        }

        private QueryExpression VisitUnary(TContext context, UnaryExpression expression, Func<QueryExpression, QueryExpression> factory)
        {
            var child = expression.Expression.Accept(this, context);
            if (!ReferenceEquals(child, expression.Expression))
            {
                return factory(child);
            }
            return expression;
        }

        private QueryExpression VisitBinary(TContext context, BinaryExpression expression, Func<QueryExpression, QueryExpression, QueryExpression> factory)
        {
            var left = expression.Left.Accept(this, context);
            var right = expression.Right.Accept(this, context);

            if (!ReferenceEquals(left, expression.Left) ||
                !ReferenceEquals(right, expression.Right))
            {
                return factory(left, right);
            }

            return expression;
        }
    }
}
