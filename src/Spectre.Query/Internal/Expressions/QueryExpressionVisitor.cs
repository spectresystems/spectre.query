using Spectre.Query.Internal.Expressions.Ast;

namespace Spectre.Query.Internal.Expressions
{
    internal abstract class QueryExpressionVisitor<TContext> : IQueryExpressionVisitor<TContext, object>
    {
        object IQueryExpressionVisitor<TContext, object>.VisitAnd(TContext context, AndExpression expression)
        {
            VisitAnd(context, expression);
            return null;
        }

        object IQueryExpressionVisitor<TContext, object>.VisitConstant(TContext context, ConstantExpression expression)
        {
            VisitConstant(context, expression);
            return null;
        }

        object IQueryExpressionVisitor<TContext, object>.VisitNot(TContext context, NotExpression expression)
        {
            VisitNot(context, expression);
            return null;
        }

        object IQueryExpressionVisitor<TContext, object>.VisitOr(TContext context, OrExpression expression)
        {
            VisitOr(context, expression);
            return null;
        }

        object IQueryExpressionVisitor<TContext, object>.VisitRelation(TContext context, RelationalExpression expression)
        {
            VisitRelational(context, expression);
            return null;
        }

        object IQueryExpressionVisitor<TContext, object>.VisitScope(TContext context, ScopeExpression expression)
        {
            VisitScope(context, expression);
            return null;
        }

        object IQueryExpressionVisitor<TContext, object>.VisitProperty(TContext context, PropertyExpression expression)
        {
            VisitProperty(context, expression);
            return null;
        }

        object IQueryExpressionVisitor<TContext, object>.VisitConversion(TContext context, ConvertExpression expression)
        {
            VisitConversion(context, expression);
            return null;
        }

        object IQueryExpressionVisitor<TContext, object>.VisitLike(TContext context, LikeExpression expression)
        {
            VisitLike(context, expression);
            return null;
        }

        object IQueryExpressionVisitor<TContext, object>.VisitCollection(TContext context, CollectionExpression expression)
        {
            VisitCollection(context, expression);
            return null;
        }

        protected abstract void VisitAnd(TContext context, AndExpression expression);
        protected abstract void VisitConstant(TContext context, ConstantExpression expression);
        protected abstract void VisitNot(TContext context, NotExpression expression);
        protected abstract void VisitOr(TContext context, OrExpression expression);
        protected abstract void VisitRelational(TContext context, RelationalExpression expression);
        protected abstract void VisitScope(TContext context, ScopeExpression expression);
        protected abstract void VisitProperty(TContext context, PropertyExpression expression);
        protected abstract void VisitConversion(TContext context, ConvertExpression expression);
        protected abstract void VisitLike(TContext context, LikeExpression expression);
        protected abstract void VisitCollection(TContext context, CollectionExpression expression);
    }

    internal abstract class QueryExpressionVisitor<TContext, TResult> : IQueryExpressionVisitor<TContext, TResult>
    {
        TResult IQueryExpressionVisitor<TContext, TResult>.VisitAnd(TContext context, AndExpression expression)
        {
            return VisitAnd(context, expression);
        }

        TResult IQueryExpressionVisitor<TContext, TResult>.VisitConstant(TContext context, ConstantExpression expression)
        {
            return VisitConstant(context, expression);
        }

        TResult IQueryExpressionVisitor<TContext, TResult>.VisitNot(TContext context, NotExpression expression)
        {
            return VisitNot(context, expression);
        }

        TResult IQueryExpressionVisitor<TContext, TResult>.VisitOr(TContext context, OrExpression expression)
        {
            return VisitOr(context, expression);
        }

        TResult IQueryExpressionVisitor<TContext, TResult>.VisitRelation(TContext context, RelationalExpression expression)
        {
            return VisitRelation(context, expression);
        }

        TResult IQueryExpressionVisitor<TContext, TResult>.VisitScope(TContext context, ScopeExpression expression)
        {
            return VisitScope(context, expression);
        }

        TResult IQueryExpressionVisitor<TContext, TResult>.VisitProperty(TContext context, PropertyExpression expression)
        {
            return VisitProperty(context, expression);
        }

        TResult IQueryExpressionVisitor<TContext, TResult>.VisitConversion(TContext context, ConvertExpression expression)
        {
            return VisitConversion(context, expression);
        }

        TResult IQueryExpressionVisitor<TContext, TResult>.VisitLike(TContext context, LikeExpression expression)
        {
            return VisitLike(context, expression);
        }

        TResult IQueryExpressionVisitor<TContext, TResult>.VisitCollection(TContext context, CollectionExpression expression)
        {
            return VisitCollection(context, expression);
        }

        protected abstract TResult VisitAnd(TContext context, AndExpression expression);
        protected abstract TResult VisitConstant(TContext context, ConstantExpression expression);
        protected abstract TResult VisitNot(TContext context, NotExpression expression);
        protected abstract TResult VisitOr(TContext context, OrExpression expression);
        protected abstract TResult VisitRelation(TContext context, RelationalExpression expression);
        protected abstract TResult VisitScope(TContext context, ScopeExpression expression);
        protected abstract TResult VisitProperty(TContext context, PropertyExpression expression);
        protected abstract TResult VisitConversion(TContext context, ConvertExpression expression);
        protected abstract TResult VisitLike(TContext context, LikeExpression expression);
        protected abstract TResult VisitCollection(TContext context, CollectionExpression expression);
    }
}
