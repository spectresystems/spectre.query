using Spectre.Query.Internal.Expressions.Ast;

namespace Spectre.Query.Internal.Expressions.Rewriting
{
    internal static class BooleanRewriter
    {
        public static QueryExpression Rewrite(QueryExpression expression)
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

        public sealed class Visitor : QueryExpressionRewriter<object>
        {
            protected override QueryExpression VisitAnd(object context, AndExpression expression)
            {
                if (expression.Left is PropertyExpression)
                {
                    // Convert single property expressions into a relational expression.
                    var left = new RelationalExpression(expression.Left, new ConstantExpression(true), RelationalOperator.EqualTo).Accept(this, context);
                    var right = expression.Right.Accept(this, context);
                    return new AndExpression(left, right);
                }
                else if (expression.Right is PropertyExpression)
                {
                    // Convert single property expressions into a relational expression.
                    var left = expression.Left.Accept(this, context);
                    var right = new RelationalExpression(expression.Right, new ConstantExpression(true), RelationalOperator.EqualTo).Accept(this, context);
                    return new AndExpression(left, right);
                }

                return base.VisitAnd(context, expression);
            }

            protected override QueryExpression VisitOr(object context, OrExpression expression)
            {
                if (expression.Left is PropertyExpression property)
                {
                    // Convert single property expressions into a relational expression.
                    var left = new RelationalExpression(expression.Left, new ConstantExpression(true), RelationalOperator.EqualTo).Accept(this, context);
                    var right = expression.Right.Accept(this, context);
                    return new OrExpression(left, right);
                }
                else if (expression.Right is PropertyExpression)
                {
                    // Convert single property expressions into a relational expression.
                    var left = expression.Left.Accept(this, context);
                    var right = new RelationalExpression(expression.Right, new ConstantExpression(true), RelationalOperator.EqualTo).Accept(this, context);
                    return new OrExpression(left, right);
                }

                return base.VisitOr(context, expression);
            }

            protected override QueryExpression VisitNot(object context, NotExpression expression)
            {
                if (expression.Expression is PropertyExpression)
                {
                    // Convert single property expressions into a relational expression.
                    var relational = new RelationalExpression(expression.Expression, new ConstantExpression(true), RelationalOperator.EqualTo).Accept(this, context);
                    return new NotExpression(relational);
                }

                return base.VisitNot(context, expression);
            }
        }
    }
}
