namespace Spectre.Query.Internal.Expressions.Ast
{
    internal sealed class ConstantExpression : Expression
    {
        public object Value { get; }

        public ConstantExpression(object value)
        {
            Value = value;
        }

        public override TResult Accept<TContext, TResult>(IExpressionVisitor<TContext, TResult> visitor, TContext context)
        {
            return visitor.VisitConstant(context, this);
        }
    }
}
