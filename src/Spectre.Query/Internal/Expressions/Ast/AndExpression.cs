namespace Spectre.Query.Internal.Expressions.Ast
{
    internal sealed class AndExpression : BinaryExpression
    {
        public AndExpression(Expression left, Expression right)
            : base(left, right)
        {
        }

        public override TResult Accept<TContext, TResult>(IExpressionVisitor<TContext, TResult> visitor, TContext context)
        {
            return visitor.VisitAnd(context, this);
        }
    }
}
