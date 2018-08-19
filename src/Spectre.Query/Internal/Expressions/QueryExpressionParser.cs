using System;
using System.Globalization;
using Spectre.Query.Internal.Configuration;
using Spectre.Query.Internal.Expressions.Ast;
using Spectre.Query.Internal.Expressions.Tokenization;

namespace Spectre.Query.Internal.Expressions
{
    internal static class QueryExpressionParser
    {
        public static QueryExpression Parse(EntityConfiguration configuration, string expression)
        {
            if (string.IsNullOrWhiteSpace(expression))
            {
                return null;
            }

            var tokenizer = new Tokenizer(expression);
            tokenizer.MoveNext();

            return Parse(configuration, tokenizer);
        }

        private static QueryExpression Parse(EntityConfiguration configuration, Tokenizer tokenizer)
        {
            return ParseOr(configuration, tokenizer);
        }

        private static QueryExpression ParseOr(EntityConfiguration configuration, Tokenizer tokenizer)
        {
            if (tokenizer.Current == null)
            {
                throw new InvalidOperationException("Invalid expression!");
            }

            var expression = ParseAnd(configuration, tokenizer);
            while (tokenizer.Current?.Type == TokenType.Or)
            {
                tokenizer.MoveNext();
                expression = new OrExpression(expression, ParseAnd(configuration, tokenizer));
            }

            return expression;
        }

        private static QueryExpression ParseAnd(EntityConfiguration configuration, Tokenizer tokenizer)
        {
            if (tokenizer.Current == null)
            {
                throw new InvalidOperationException("Invalid expression!");
            }

            var expression = ParsePredicate(configuration, tokenizer);
            while (tokenizer.Current?.Type == TokenType.And)
            {
                tokenizer.MoveNext();
                expression = new AndExpression(expression, ParsePredicate(configuration, tokenizer));
            }

            return expression;
        }

        private static QueryExpression ParsePredicate(EntityConfiguration configuration, Tokenizer tokenizer)
        {
            if (tokenizer.Current == null)
            {
                throw new InvalidOperationException("Invalid expression!");
            }

            if (tokenizer.Current.Type == TokenType.Not)
            {
                tokenizer.MoveNext();
                return new NotExpression(ParsePredicate(configuration, tokenizer));
            }

            return ParseRelation(configuration, tokenizer);
        }

        private static QueryExpression ParseRelation(EntityConfiguration configuration, Tokenizer tokenizer)
        {
            if (tokenizer.Current == null)
            {
                throw new InvalidOperationException("Invalid expression!");
            }

            // Parse the left side literal in the relation.
            var expression = ParseLiteral(configuration, tokenizer);

            if (tokenizer.Current?.IsRelationalOperator() == true)
            {
                var @operator = tokenizer.Current.GetRelationalOperator();
                tokenizer.MoveNext(); // Consume operator.

                // Parse the right side literal in the relation.
                var right = ParseLiteral(configuration, tokenizer);

                return new RelationalExpression(
                    expression,
                    right,
                    @operator);
            }

            return expression;
        }

        private static QueryExpression ParseLiteral(EntityConfiguration configuration, Tokenizer tokenizer)
        {
            var result = (QueryExpression)null;
            if (tokenizer.Current != null)
            {
                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (tokenizer.Current.Type)
                {
                    case TokenType.Integer:
                        result = ParseConstant(tokenizer, value => int.Parse(value, CultureInfo.InvariantCulture));
                        break;
                    case TokenType.Decimal:
                        result = ParseConstant(tokenizer, value => decimal.Parse(value, CultureInfo.InvariantCulture));
                        break;
                    case TokenType.String:
                        result = ParseConstant(tokenizer, value => value);
                        break;
                    case TokenType.True:
                        result = ParseConstant(tokenizer, _ => true);
                        break;
                    case TokenType.False:
                        result = ParseConstant(tokenizer, _ => false);
                        break;
                    case TokenType.Null:
                        result = ParseConstant(tokenizer, _ => null);
                        break;
                    case TokenType.OpeningParenthesis:
                        result = ParseScope(configuration, tokenizer);
                        break;
                    case TokenType.Word:
                        result = ParseProperty(configuration, tokenizer);
                        break;
                    default:
                        throw new InvalidOperationException($"Could not parse literal expression ({tokenizer.Current.Type}).");
                }
            }

            if (result != null)
            {
                return result;
            }

            throw new InvalidOperationException("Unexpected end of expression.");
        }

        private static QueryExpression ParseProperty(EntityConfiguration configuration, Tokenizer tokenizer)
        {
            var name = tokenizer.Consume(TokenType.Word).Value;

            var property = configuration.GetProperty(name);
            if (property == null)
            {
                throw new InvalidOperationException("Could not find property.");
            }

            return new PropertyExpression(
                property.EntityType,
                property.Properties);
        }

        private static QueryExpression ParseConstant(Tokenizer tokenizer, Func<string, object> converter)
        {
            var expression = new ConstantExpression(converter(tokenizer.Current.Value));
            tokenizer.MoveNext(); // Move past constant.

            return expression;
        }

        private static QueryExpression ParseScope(EntityConfiguration configuration, Tokenizer tokenizer)
        {
            tokenizer.Consume(TokenType.OpeningParenthesis);
            var expression = Parse(configuration, tokenizer);
            tokenizer.Consume(TokenType.ClosingParenthesis);

            return new ScopeExpression(expression);
        }
    }
}
