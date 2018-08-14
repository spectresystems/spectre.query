using System;
using System.Reflection;

namespace Spectre.Query.Internal.Expressions.Ast
{
    internal sealed class PropertyExpression : QueryExpression
    {
        public string Name { get; }
        public Type Type { get; }
        public bool IsNullable { get; }

        public PropertyExpression(PropertyInfo property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            Name = property.Name;
            Type = property.PropertyType;
            IsNullable = Nullable.GetUnderlyingType(Type) != null;
        }

        public override TResult Accept<TContext, TResult>(IQueryExpressionVisitor<TContext, TResult> visitor, TContext context)
        {
            return visitor.VisitProperty(context, this);
        }
    }
}
