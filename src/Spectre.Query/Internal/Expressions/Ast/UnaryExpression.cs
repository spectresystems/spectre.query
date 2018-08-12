namespace Spectre.Query.Internal.Expressions.Ast
{
    internal abstract class UnaryExpression : QueryExpression
    {
        public QueryExpression Expression { get; }

        protected UnaryExpression(QueryExpression expression)
        {
            Expression = expression;
        }
    }
}
