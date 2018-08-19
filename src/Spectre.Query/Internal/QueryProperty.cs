using System;
using System.Collections.Generic;
using System.Reflection;

namespace Spectre.Query.Internal
{
    internal sealed class QueryProperty
    {
        public Type EntityType { get; set; }
        public string Alias { get; set; }
        public IReadOnlyList<PropertyInfo> Properties { get; set; }

        public QueryProperty(string alias, Type entityType, IReadOnlyList<PropertyInfo> properties)
        {
            EntityType = entityType;
            Alias = alias;
            Properties = properties;
        }
    }
}
