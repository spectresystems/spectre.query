using System;
using System.Collections.Generic;
using System.Reflection;

namespace Spectre.Query.Internal.Expressions.Ast
{
    internal sealed class PropertyExpression : QueryExpression
    {
        public Type EntityType { get; }
        public Type PropertyType { get; }
        public IReadOnlyList<PropertyInfo> Properties { get; }
        public bool IsNullable { get; }

        public PropertyExpression(Type objectType, IReadOnlyList<PropertyInfo> properties)
        {
            EntityType = objectType;
            PropertyType = properties[properties.Count - 1].PropertyType;
            Properties = properties;
            IsNullable = Nullable.GetUnderlyingType(PropertyType) != null;
        }

        public override TResult Accept<TContext, TResult>(IQueryExpressionVisitor<TContext, TResult> visitor, TContext context)
        {
            return visitor.VisitProperty(context, this);
        }
    }
}
