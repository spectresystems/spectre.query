using System;
using System.Reflection;

namespace Spectre.Query.Internal.Expressions.Ast
{
    internal sealed class PropertyExpression : QueryExpression
    {
        public string Name { get; }
        public Type EntityType { get; }
        public Type PropertyType { get; }
        public bool IsNullable { get; }

        public PropertyExpression(Type objectType, PropertyInfo property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            Name = property.Name;
            EntityType = objectType;
            PropertyType = property.PropertyType;
            IsNullable = Nullable.GetUnderlyingType(PropertyType) != null;
        }

        public override TResult Accept<TContext, TResult>(IQueryExpressionVisitor<TContext, TResult> visitor, TContext context)
        {
            return visitor.VisitProperty(context, this);
        }
    }
}
