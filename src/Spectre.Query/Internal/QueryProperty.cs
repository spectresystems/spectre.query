using System;
using System.Collections.Generic;
using System.Reflection;
using Spectre.Query.Internal.Expressions;
using Spectre.Query.Internal.Expressions.Ast;

namespace Spectre.Query.Internal
{
    internal sealed class QueryProperty : IQueryMapping
    {
        public string Alias { get; set; }
        public Type EntityType { get; set; }
        public IReadOnlyList<PropertyInfo> Properties { get; set; }

        public QueryProperty(string alias, Type entityType, IReadOnlyList<PropertyInfo> properties)
        {
            EntityType = entityType;
            Alias = alias;
            Properties = properties;
        }

        public QueryExpression CreateExpression()
        {
            return new PropertyExpression(EntityType, Properties);
        }
    }
}
