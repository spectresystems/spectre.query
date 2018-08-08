namespace Spectre.Query.Internal.Expressions
{
    internal abstract class QueryExpression
    {
        public abstract TResult Accept<TContext, TResult>(IQueryExpressionVisitor<TContext, TResult> visitor, TContext context);
    }
}
