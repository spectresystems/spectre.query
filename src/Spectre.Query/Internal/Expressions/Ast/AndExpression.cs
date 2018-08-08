namespace Spectre.Query.Internal.Expressions.Ast
{
    internal sealed class AndExpression : BinaryExpression
    {
        public AndExpression(QueryExpression left, QueryExpression right)
            : base(left, right)
        {
        }

        public override TResult Accept<TContext, TResult>(IQueryExpressionVisitor<TContext, TResult> visitor, TContext context)
        {
            return visitor.VisitAnd(context, this);
        }
    }
}
