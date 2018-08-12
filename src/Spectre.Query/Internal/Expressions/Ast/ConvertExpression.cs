using System;

namespace Spectre.Query.Internal.Expressions.Ast
{
    internal sealed class ConvertExpression : UnaryExpression
    {
        public Type Type { get; }

        public ConvertExpression(QueryExpression expression, Type type)
            : base(expression)
        {
            Type = type;
        }

        public override TResult Accept<TContext, TResult>(IQueryExpressionVisitor<TContext, TResult> visitor, TContext context)
        {
            return visitor.VisitConversion(context, this);
        }
    }
}
