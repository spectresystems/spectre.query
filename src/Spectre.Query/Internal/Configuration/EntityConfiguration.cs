using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Spectre.Query.Internal.Configuration
{
    internal sealed class EntityConfiguration
    {
        private readonly List<QueryProperty> _mappings;

        public Type Type { get; }
        public bool IsQueryType { get; }
        public IReadOnlyList<QueryProperty> Mappings => _mappings;

        public EntityConfiguration(IEntityType entityType, Type type)
        {
            _mappings = new List<QueryProperty>();

            Type = type;
            IsQueryType = entityType.IsQueryType;
        }

        public void AddProperty(QueryProperty property)
        {
            if (GetProperty(property.Alias) != null)
            {
                throw new InvalidOperationException($"The property '{property.Alias}' have been defined twice.");
            }

            _mappings.Add(property);
        }

        public QueryProperty GetProperty(string name)
        {
            return _mappings.Find(m => m.Alias.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
    }
}
