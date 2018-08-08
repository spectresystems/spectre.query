namespace Spectre.Query.Internal.Expressions.Ast
{
    internal sealed class PropertyExpression : Expression
    {
        public string Name { get; set; }

        public PropertyExpression(string name)
        {
            Name = name;
        }

        public override TResult Accept<TContext, TResult>(IExpressionVisitor<TContext, TResult> visitor, TContext context)
        {
            return visitor.VisitProperty(context, this);
        }
    }
}
