namespace Spectre.Query.Internal.Expressions
{
    internal abstract class Expression
    {
        public abstract TResult Accept<TContext, TResult>(IExpressionVisitor<TContext, TResult> visitor, TContext context);
    }
}
