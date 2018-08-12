using System;
using Spectre.Query.Internal.Expressions.Ast;

namespace Spectre.Query.Internal.Expressions.Rewriting
{
    internal static class NullableRewriter
    {
        public static QueryExpression Rewrite(QueryExpression expression)
        {
            if (expression == null)
            {
                return null;
            }

            return expression.Accept(new Visitor(), null);
        }

        public sealed class Visitor : QueryExpressionRewriter<object>
        {
            protected override QueryExpression VisitRelational(object context, RelationalExpression expression)
            {
                // Left hand side is a nullable property?
                if (expression.Left is PropertyExpression property && property.IsNullable)
                {
                    // Don't care if the right side is a null constant.
                    if (!(expression.Right is ConstantExpression constant && constant == null))
                    {
                        // Try converting the right side of the expression.
                        return new RelationalExpression(
                            expression.Left,
                            new ConvertExpression(expression.Right.Accept(this, context), property.Type),
                            expression.Operator);
                    }
                }

                return base.VisitRelational(context, expression);
            }
        }
    }
}
