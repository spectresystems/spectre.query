namespace Spectre.Query.Internal.Expressions.Ast
{
    internal sealed class ScopeExpression : UnaryExpression
    {
        public ScopeExpression(QueryExpression expression)
            : base(expression)
        {
        }

        public override TResult Accept<TContext, TResult>(IQueryExpressionVisitor<TContext, TResult> visitor, TContext context)
        {
            return visitor.VisitScope(context, this);
        }
    }
}
