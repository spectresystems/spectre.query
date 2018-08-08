using Spectre.Query.Internal.Expressions.Ast;

namespace Spectre.Query.Internal.Expressions
{
    internal abstract class ExpressionVisitor<TContext, TResult> : IExpressionVisitor<TContext, TResult>
    {
        TResult IExpressionVisitor<TContext, TResult>.VisitAnd(TContext context, AndExpression expression)
        {
            return VisitAnd(context, expression);
        }

        TResult IExpressionVisitor<TContext, TResult>.VisitConstant(TContext context, ConstantExpression expression)
        {
            return VisitConstant(context, expression);
        }

        TResult IExpressionVisitor<TContext, TResult>.VisitNot(TContext context, NotExpression expression)
        {
            return VisitNot(context, expression);
        }

        TResult IExpressionVisitor<TContext, TResult>.VisitOr(TContext context, OrExpression expression)
        {
            return VisitOr(context, expression);
        }

        TResult IExpressionVisitor<TContext, TResult>.VisitRelational(TContext context, RelationalExpression expression)
        {
            return VisitRelational(context, expression);
        }

        TResult IExpressionVisitor<TContext, TResult>.VisitScope(TContext context, ScopeExpression expression)
        {
            return VisitScope(context, expression);
        }

        TResult IExpressionVisitor<TContext, TResult>.VisitProperty(TContext context, PropertyExpression expression)
        {
            return VisitProperty(context, expression);
        }

        protected abstract TResult VisitAnd(TContext context, AndExpression expression);
        protected abstract TResult VisitConstant(TContext context, ConstantExpression expression);
        protected abstract TResult VisitNot(TContext context, NotExpression expression);
        protected abstract TResult VisitOr(TContext context, OrExpression expression);
        protected abstract TResult VisitRelational(TContext context, RelationalExpression expression);
        protected abstract TResult VisitScope(TContext context, ScopeExpression expression);
        protected abstract TResult VisitProperty(TContext context, PropertyExpression expression);
    }
}
