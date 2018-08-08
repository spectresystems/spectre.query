namespace Spectre.Query.Internal.Expressions.Ast
{
    internal abstract class UnaryExpression : Expression
    {
        public Expression Expression { get; }

        protected UnaryExpression(Expression expression)
        {
            Expression = expression;
        }
    }
}
