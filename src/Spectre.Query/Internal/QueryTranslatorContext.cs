using System.Linq.Expressions;

namespace Spectre.Query.Internal
{
    internal sealed class QueryTranslatorContext
    {
        public ParameterExpression Parameter { get; }

        public QueryTranslatorContext(ParameterExpression parameter)
        {
            Parameter = parameter;
        }
    }
}
