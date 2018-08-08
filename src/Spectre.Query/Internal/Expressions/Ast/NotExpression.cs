namespace Spectre.Query.Internal.Expressions.Ast
{
    internal sealed class NotExpression : UnaryExpression
    {
        public NotExpression(Expression expression)
            : base(expression)
        {
        }

        public override TResult Accept<TContext, TResult>(IExpressionVisitor<TContext, TResult> visitor, TContext context)
        {
            return visitor.VisitNot(context, this);
        }
    }
}
