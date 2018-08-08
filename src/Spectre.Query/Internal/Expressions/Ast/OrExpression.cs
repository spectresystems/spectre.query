namespace Spectre.Query.Internal.Expressions.Ast
{
    internal sealed class OrExpression : BinaryExpression
    {
        public OrExpression(QueryExpression left, QueryExpression right)
            : base(left, right)
        {
        }

        public override TResult Accept<TContext, TResult>(IQueryExpressionVisitor<TContext, TResult> visitor, TContext context)
        {
            return visitor.VisitOr(context, this);
        }
    }
}
