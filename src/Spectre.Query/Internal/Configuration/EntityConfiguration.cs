using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Spectre.Query.Internal.Configuration
{
    internal sealed class EntityConfiguration
    {
        public Type Type { get; }
        public bool IsQueryType { get; }
        public string TableName { get; }
        public List<QueryProperty> Mappings { get; }

        public EntityConfiguration(IEntityType entityType, Type type)
        {
            Type = type;
            TableName = entityType.Relational().TableName;
            IsQueryType = entityType.IsQueryType;
            Mappings = new List<QueryProperty>();
        }

        public QueryProperty GetProperty(string name)
        {
            return Mappings.Find(m => m.Alias.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
    }
}
