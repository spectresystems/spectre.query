using System;
using System.Collections.Generic;
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

            var property = GetPropertyInfo(member);
            var entityProperty = _entityType.FindProperty(property);
            if (entityProperty == null)
            {
                throw new InvalidOperationException($"The property '{typeof(TEntity).Name}.{property.Name}' is not mapped to entity.");
            }

            Configuration.AddProperty(new QueryProperty
            {
                EntityType = typeof(TEntity),
                Alias = name,
                PropertyInfo = property
            });
        }

        private static PropertyInfo GetPropertyInfo(MemberExpression member)
        {
            var parts = new List<PropertyInfo>();
            while (member?.Expression != null)
            {
                // Get the property info.
                if (!(member.Member is PropertyInfo property))
                {
                    throw new InvalidOperationException("Only properties can be mapped.");
                }
                parts.Add(property);
                member = member.Expression as MemberExpression;
            }

            if (parts.Count != 1)
            {
                throw new InvalidOperationException("Navigational properties are not allowed.");
            }

            return parts[0];
        }
    }
}
