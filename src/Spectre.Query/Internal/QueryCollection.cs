using System.Collections.Generic;
using System.Reflection;
using Spectre.Query.Internal.Expressions;
using Spectre.Query.Internal.Expressions.Ast;

namespace Spectre.Query.Internal
{
    internal sealed class QueryCollection : IQueryMapping
    {
        public string Alias { get; set; }
        public IReadOnlyList<PropertyInfo> Properties { get; set; }
        public IReadOnlyList<PropertyInfo> Accessor { get; set; }

        public QueryCollection(
            string alias,
            IReadOnlyList<PropertyInfo> collectionProperties,
            IReadOnlyList<PropertyInfo> getterProperties)
        {
            Alias = alias;
            Properties = collectionProperties;
            Accessor = getterProperties;
        }

        public QueryExpression CreateExpression()
        {
            return new CollectionExpression(
                Properties,
                Accessor);
        }
    }
}
