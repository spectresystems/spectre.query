using System;
using System.Collections.Generic;
using System.Reflection;

namespace Spectre.Query.Internal.Expressions.Ast
{
    internal sealed class CollectionExpression : QueryExpression
    {
        public IReadOnlyList<PropertyInfo> Properties { get; }

        public Type ItemType { get; }
        public PropertyExpression ItemAccessor { get; }

        public CollectionExpression(IReadOnlyList<PropertyInfo> properties, IReadOnlyList<PropertyInfo> getter)
        {
            Properties = properties;

            ItemType = properties[properties.Count - 1].PropertyType.GenericTypeArguments[0];
            ItemAccessor = new PropertyExpression(ItemType, getter);
        }

        public override TResult Accept<TContext, TResult>(IQueryExpressionVisitor<TContext, TResult> visitor, TContext context)
        {
            return visitor.VisitCollection(context, this);
        }
    }
}
