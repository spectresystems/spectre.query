﻿using Spectre.Query.Internal.Expressions.Ast;

namespace Spectre.Query.Internal.Expressions.Rewriting
{
    internal static class ConversionRewriter
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
                if (expression.Left is PropertyExpression property)
                {
                    if (property.IsNullable)
                    {
                        return RewriteNullableProperty(context, expression, property);
                    }
                    else
                    {
                        return RewriteProperty(context, expression, property);
                    }
                }

                return base.VisitRelational(context, expression);
            }

            private QueryExpression RewriteNullableProperty(object context, RelationalExpression expression, PropertyExpression property)
            {
                // Don't care if the right side is a null constant.
                if (expression.Right is ConstantExpression constant && constant == null)
                {
                    return base.VisitRelational(context, expression);
                }

                // Try converting the right side of the expression.
                return new RelationalExpression(
                    expression.Left,
                    new ConvertExpression(expression.Right.Accept(this, context), property.Type),
                    expression.Operator);
            }

            private QueryExpression RewriteProperty(object context, RelationalExpression expression, PropertyExpression property)
            {
                // Is the right side a non-null constant?
                if (expression.Right is ConstantExpression constant && constant.Value != null)
                {
                    // Check if they're not the same type.
                    if (property.Type != constant.Value.GetType())
                    {
                        // Try converting the right side of the expression to the property's type.
                        return new RelationalExpression(
                            expression.Left,
                            new ConvertExpression(expression.Right.Accept(this, context), property.Type),
                            expression.Operator);
                    }
                }
                else
                {
                    // Try converting the right side of the expression to the property's type.
                    return new RelationalExpression(
                        expression.Left,
                        new ConvertExpression(expression.Right.Accept(this, context), property.Type),
                        expression.Operator);
                }

                return base.VisitRelational(context, expression);
            }
        }
    }
}
