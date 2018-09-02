namespace Spectre.Query.Internal.Expressions.Ast
{
    internal sealed class LikeExpression : BinaryExpression
    {
        public LikeExpression(QueryExpression left, QueryExpression right)
            : base(left, right)
        {
        }

        public override TResult Accept<TContext, TResult>(IQueryExpressionVisitor<TContext, TResult> visitor, TContext context)
        {
            return visitor.VisitLike(context, this);
        }
    }
}
