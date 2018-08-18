using System;
using System.Linq.Expressions;

namespace Spectre.Query.Internal
{
    internal sealed class QueryTranslatorContext
    {
        public Type RootType { get; }
        public ParameterExpression Parameter { get; }

        public QueryTranslatorContext(Type rootType)
        {
            RootType = rootType;
            Parameter = Expression.Parameter(rootType, "entity");
        }
    }
}
