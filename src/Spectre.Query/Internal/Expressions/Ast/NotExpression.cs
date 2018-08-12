namespace Spectre.Query.Internal.Expressions.Ast
{
    internal sealed class NotExpression : UnaryExpression
    {
        public NotExpression(QueryExpression expression)
            : base(expression)
        {
        }

        public override TResult Accept<TContext, TResult>(IQueryExpressionVisitor<TContext, TResult> visitor, TContext context)
        {
            return visitor.VisitNot(context, this);
        }
    }
}
