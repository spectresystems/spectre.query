using Spectre.Query.Internal.Expressions.Ast;

namespace Spectre.Query.Internal.Expressions.Rewriting
{
    internal static class NullRewriter<TEntity>
    {
        public static Expression Rewrite(Expression expression)
        {
            if (expression == null)
            {
                return null;
            }

            expression = expression.Accept(new Visitor(), null);
            if (expression is PropertyExpression propertyExpression)
            {
                // A single property expression left?
                // Convert that one into a relational expression.
                expression = new RelationalExpression(propertyExpression, new ConstantExpression(true), RelationalOperator.EqualTo);
            }

            return expression;
        }

        public sealed class Visitor : ExpressionRewriter<object>
        {
            protected override Expression VisitRelational(object context, RelationalExpression expression)
            {
                // Rewrite [Foo == NULL] to [Foo IS NULL]
                if (expression.Operator == RelationalOperator.EqualTo)
                {
                    if (expression.Right is ConstantExpression constant)
                    {
                        if (constant.Value == null)
                        {
                            return new RelationalExpression(
                                expression.Left,
                                expression.Right,
                                RelationalOperator.Is).Accept(this, context);
                        }
                    }
                }

                // Rewrite [Foo != NULL] to [Foo IS NOT NULL]
                if (expression.Operator == RelationalOperator.NotEqualTo)
                {
                    if (expression.Right is ConstantExpression constant)
                    {
                        if (constant.Value == null)
                        {
                            return new RelationalExpression(
                                expression.Left,
                                expression.Right,
                                RelationalOperator.IsNot).Accept(this, context);
                        }
                    }
                }

                return expression;
            }
        }
    }
}
