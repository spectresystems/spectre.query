namespace Spectre.Query.Internal.Expressions.Ast
{
    internal sealed class OrExpression : BinaryExpression
    {
        public OrExpression(Expression left, Expression right)
            : base(left, right)
        {
        }

        public override TResult Accept<TContext, TResult>(IExpressionVisitor<TContext, TResult> visitor, TContext context)
        {
            return visitor.VisitOr(context, this);
        }
    }
}
