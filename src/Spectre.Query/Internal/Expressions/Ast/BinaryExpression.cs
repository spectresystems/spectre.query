namespace Spectre.Query.Internal.Expressions.Ast
{
    internal abstract class BinaryExpression : QueryExpression
    {
        public QueryExpression Left { get; }
        public QueryExpression Right { get; }

        protected BinaryExpression(QueryExpression left, QueryExpression right)
        {
            Left = left;
            Right = right;
        }
    }
}
