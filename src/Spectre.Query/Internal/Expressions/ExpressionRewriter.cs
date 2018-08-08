using System;
using Spectre.Query.Internal.Expressions.Ast;

namespace Spectre.Query.Internal.Expressions
{
    internal abstract class ExpressionRewriter<TContext> : ExpressionVisitor<TContext, Expression>
    {
        protected override Expression VisitAnd(TContext context, AndExpression expression)
        {
            return VisitBinary(context, expression, (left, right) =>
                new AndExpression(left, right));
        }

        protected override Expression VisitConstant(TContext context, ConstantExpression expression)
        {
            return expression;
        }

        protected override Expression VisitNot(TContext context, NotExpression expression)
        {
            return VisitUnary(context, expression, child =>
                new NotExpression(child));
        }

        protected override Expression VisitOr(TContext context, OrExpression expression)
        {
            return VisitBinary(context, expression, (left, right) =>
                new OrExpression(left, right));
        }

        protected override Expression VisitRelational(TContext context, RelationalExpression expression)
        {
            return VisitBinary(context, expression, (left, right) =>
                new RelationalExpression(left, right, expression.Operator));
        }

        protected override Expression VisitScope(TContext context, ScopeExpression expression)
        {
            return VisitUnary(context, expression, child =>
                new ScopeExpression(child));
        }

        protected override Expression VisitProperty(TContext context, PropertyExpression expression)
        {
            return expression;
        }

        private Expression VisitUnary(TContext context, UnaryExpression expression, Func<Expression, Expression> factory)
        {
            var child = expression.Expression.Accept(this, context);
            if (!ReferenceEquals(child, expression.Expression))
            {
                return factory(child);
            }
            return expression;
        }

        private Expression VisitBinary(TContext context, BinaryExpression expression, Func<Expression, Expression, Expression> factory)
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
