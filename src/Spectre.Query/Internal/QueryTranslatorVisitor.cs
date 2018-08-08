using System;
using System.Collections.Generic;
using Spectre.Query.Internal.Configuration;
using Spectre.Query.Internal.Expressions;
using Spectre.Query.Internal.Expressions.Ast;

namespace Spectre.Query.Internal
{
    internal sealed class QueryTranslatorVisitor : ExpressionVisitor<EntityConfiguration, string>
    {
        private readonly Dictionary<RelationalOperator, string> _operatorNames;

        public QueryTranslatorVisitor()
        {
            _operatorNames = new Dictionary<RelationalOperator, string>
            {
                { RelationalOperator.Is, "IS" },
                { RelationalOperator.IsNot, "IS NOT" },
                { RelationalOperator.EqualTo, "=" },
                { RelationalOperator.NotEqualTo, "<>" },
                { RelationalOperator.GreaterThanOrEqualTo, ">=" },
                { RelationalOperator.GreaterThan, ">" },
                { RelationalOperator.LessThanOrEqualTo, "<=" },
                { RelationalOperator.LessThan, "<" },
            };
        }

        protected override string VisitAnd(EntityConfiguration context, AndExpression expression)
        {
            return string.Concat(
                expression.Left.Accept(this, context),
                " AND ",
                expression.Right.Accept(this, context));
        }

        protected override string VisitConstant(EntityConfiguration context, ConstantExpression expression)
        {
            if (expression.Value is string stringValue)
            {
                return string.Concat("'", stringValue, "'");
            }
            if (expression.Value is bool boolValue)
            {
                return string.Concat(boolValue ? "1" : "0");
            }
            if (expression.Value == null)
            {
                return "NULL";
            }
            return expression.Value.ToString();
        }

        protected override string VisitNot(EntityConfiguration context, NotExpression expression)
        {
            return string.Concat("NOT ", expression.Expression.Accept(this, context));
        }

        protected override string VisitOr(EntityConfiguration context, OrExpression expression)
        {
            return string.Concat(
                expression.Left.Accept(this, context),
                " OR ",
                expression.Right.Accept(this, context));
        }

        protected override string VisitProperty(EntityConfiguration context, PropertyExpression expression)
        {
            var property = context.Mappings.Find(m => m.Name.Equals(expression.Name, StringComparison.OrdinalIgnoreCase));
            if (property == null)
            {
                throw new InvalidOperationException($"The property {expression.Name} has not been mapped.");
            }

            return $"{property.TableName}.{property.ColumnName}";
        }

        protected override string VisitRelational(EntityConfiguration context, RelationalExpression expression)
        {
            var left = expression.Left.Accept(this, context);
            var right = expression.Right.Accept(this, context);

            if (!_operatorNames.TryGetValue(expression.Operator, out var op))
            {
                throw new InvalidOperationException($"Unknown relational operator '{expression.Operator}'.");
            }

            return string.Concat(left, " ", op, " ", right);
        }

        protected override string VisitScope(EntityConfiguration context, ScopeExpression expression)
        {
            return string.Concat("(", expression.Expression.Accept(this, context), ")");
        }
    }
}
