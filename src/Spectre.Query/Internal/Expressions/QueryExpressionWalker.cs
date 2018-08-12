using Spectre.Query.Internal.Expressions.Ast;

namespace Spectre.Query.Internal.Expressions
{
    internal abstract class QueryExpressionWalker<TContext> : QueryExpressionVisitor<TContext>
    {
        protected override void VisitAnd(TContext context, AndExpression expression)
        {
            expression.Left.Accept(this, context);
            expression.Right.Accept(this, context);
        }

        protected override void VisitConstant(TContext context, ConstantExpression expression)
        {
        }

        protected override void VisitNot(TContext context, NotExpression expression)
        {
            expression.Expression.Accept(this, context);
        }

        protected override void VisitOr(TContext context, OrExpression expression)
        {
            expression.Left.Accept(this, context);
            expression.Right.Accept(this, context);
        }

        protected override void VisitProperty(TContext context, PropertyExpression expression)
        {
        }

        protected override void VisitRelational(TContext context, RelationalExpression expression)
        {
            expression.Left.Accept(this, context);
            expression.Right.Accept(this, context);
        }

        protected override void VisitScope(TContext context, ScopeExpression expression)
        {
            expression.Expression.Accept(this, context);
        }
    }
}
