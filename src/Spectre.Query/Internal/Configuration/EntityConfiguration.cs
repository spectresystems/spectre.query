using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Spectre.Query.Internal.Configuration
{
    internal sealed class EntityConfiguration
    {
        private readonly List<IQueryMapping> _mappings;

        public Type Type { get; }
        public bool IsQueryType { get; }
        public IReadOnlyList<IQueryMapping> Mappings => _mappings;

        public EntityConfiguration(IEntityType entityType, Type type)
        {
            _mappings = new List<IQueryMapping>();

            Type = type;
            IsQueryType = entityType.IsQueryType;
        }

        public void AddProperty(IQueryMapping property)
        {
            if (GetMapping(property.Alias) != null)
            {
                throw new InvalidOperationException($"The property '{property.Alias}' have been defined twice.");
            }

            _mappings.Add(property);
        }

        public IQueryMapping GetMapping(string name)
        {
            return _mappings.Find(m => m.Alias.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
    }
}
