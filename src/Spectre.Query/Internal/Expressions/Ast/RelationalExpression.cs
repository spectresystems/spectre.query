namespace Spectre.Query.Internal.Expressions.Ast
{
    internal sealed class RelationalExpression : BinaryExpression
    {
        public RelationalOperator Operator { get; }

        public RelationalExpression(Expression left, Expression right, RelationalOperator @operator)
            : base(left, right)
        {
            Operator = @operator;
        }

        public override TResult Accept<TContext, TResult>(IExpressionVisitor<TContext, TResult> visitor, TContext context)
        {
            return visitor.VisitRelational(context, this);
        }
    }
}
