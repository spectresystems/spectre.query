using Spectre.Query.Internal.Expressions.Ast;

namespace Spectre.Query.Internal.Expressions
{
    internal interface IQueryExpressionVisitor<in TContext, out TResult>
    {
        TResult VisitAnd(TContext context, AndExpression expression);
        TResult VisitConstant(TContext context, ConstantExpression expression);
        TResult VisitNot(TContext context, NotExpression expression);
        TResult VisitOr(TContext context, OrExpression expression);
        TResult VisitRelational(TContext context, RelationalExpression expression);
        TResult VisitScope(TContext context, ScopeExpression expression);
        TResult VisitProperty(TContext context, PropertyExpression expression);
        TResult VisitConversion(TContext context, ConvertExpression expression);
    }
}
