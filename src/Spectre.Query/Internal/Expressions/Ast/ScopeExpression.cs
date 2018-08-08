namespace Spectre.Query.Internal.Expressions.Ast
{
    internal sealed class ScopeExpression : UnaryExpression
    {
        public ScopeExpression(Expression expression)
            : base(expression)
        {
        }

        public override TResult Accept<TContext, TResult>(IExpressionVisitor<TContext, TResult> visitor, TContext context)
        {
            return visitor.VisitScope(context, this);
        }
    }
}
