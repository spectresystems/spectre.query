using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Spectre.Query.Internal.Configuration
{
    internal sealed class EntityConfigurator<TContext, TEntity> : IRootEntityConfigurator<TEntity>
        where TContext : DbContext
    {
        private readonly QueryConfigurator<TContext> _parent;
        private readonly TContext _context;
        private readonly IEntityType _entityType;

        public EntityConfiguration Configuration { get; }

        public EntityConfigurator(QueryConfigurator<TContext> parent, TContext context, IEntityType entityType)
        {
            _parent = parent;
            _context = context;
            _entityType = entityType;

            Configuration = new EntityConfiguration(_entityType, typeof(TEntity));
        }

        public void Map<TDerivedEntity>(Action<IEntityConfigurator<TDerivedEntity>> options)
            where TDerivedEntity : TEntity
        {
            // Configure the entity as a base type.
            var configuration = _parent.Configure(options);

            // Copy the mappings to this configuration.
            foreach (var mapping in configuration.Mappings)
            {
                Configuration.AddProperty(mapping);
            }
        }

        public void Map<TResult>(string name, Expression<Func<TEntity, TResult>> expression)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            if (!(expression.Body is MemberExpression member))
            {
                throw new InvalidOperationException("Expected member expression");
            }

            var properties = GetPropertyInfo(member);
            if (properties.Count == 0)
            {
                throw new InvalidOperationException("Could not resolve property from expression.");
            }

            // Get the last property of the expression.
            var property = properties[properties.Count - 1];

            // Is the last property a navigational property?
            if (_context.Model.FindEntityType(property.PropertyType) != null)
            {
                throw new InvalidOperationException($"Cannot map the navigational property '{property.Name}' directly.");
            }

            // Make sure that the property is a property on the entity.
            var entityType = _context.Model.FindEntityType(property.DeclaringType) ?? _entityType;
            var entityProperty = entityType.FindProperty(property);
            if (entityProperty == null)
            {
                var path = string.Join(".", properties.Select(x => x.Name));
                throw new InvalidOperationException($"The property '{entityType.ClrType.Name}.{path}' is not mapped to an entity.");
            }

            Configuration.AddProperty(
                new QueryProperty(name, _entityType.ClrType, properties));
        }

        private static IReadOnlyList<PropertyInfo> GetPropertyInfo(MemberExpression member)
        {
            var parts = new Stack<PropertyInfo>();
            while (member?.Expression != null)
            {
                if (!(member.Member is PropertyInfo property))
                {
                    throw new InvalidOperationException("Only properties can be mapped.");
                }
                parts.Push(property);
                member = member.Expression as MemberExpression;
            }

            return parts.ToArray();
        }
    }
}
