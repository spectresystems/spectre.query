using System;
using Spectre.Query.Internal.Expressions.Ast;
using Spectre.Query.Internal.Expressions.Tokenization;

namespace Spectre.Query.Internal.Expressions
{
    internal static class ExpressionParser
    {
        public static Expression Parse(string expression)
        {
            if (string.IsNullOrWhiteSpace(expression))
            {
                return null;
            }

            var tokenizer = new Tokenizer(expression);
            tokenizer.MoveNext();

            return Parse(tokenizer);
        }

        private static Expression Parse(Tokenizer tokenizer)
        {
            return ParseOr(tokenizer);
        }

        private static Expression ParseOr(Tokenizer tokenizer)
        {
            if (tokenizer.Current == null)
            {
                throw new InvalidOperationException("Invalid expression!");
            }

            var expression = ParseAnd(tokenizer);
            while (tokenizer.Current?.Type == TokenType.Or)
            {
                tokenizer.MoveNext();
                expression = new OrExpression(expression, ParseAnd(tokenizer));
            }

            return expression;
        }

        private static Expression ParseAnd(Tokenizer tokenizer)
        {
            if (tokenizer.Current == null)
            {
                throw new InvalidOperationException("Invalid expression!");
            }

            var expression = ParsePredicate(tokenizer);
            while (tokenizer.Current?.Type == TokenType.And)
            {
                tokenizer.MoveNext();
                expression = new AndExpression(expression, ParsePredicate(tokenizer));
            }

            return expression;
        }

        private static Expression ParsePredicate(Tokenizer tokenizer)
        {
            if (tokenizer.Current == null)
            {
                throw new InvalidOperationException("Invalid expression!");
            }

            if (tokenizer.Current.Type == TokenType.Not)
            {
                tokenizer.MoveNext();
                return new NotExpression(ParsePredicate(tokenizer));
            }

            return ParseRelation(tokenizer);
        }

        private static Expression ParseRelation(Tokenizer tokenizer)
        {
            if (tokenizer.Current == null)
            {
                throw new InvalidOperationException("Invalid expression!");
            }

            // Parse the left side literal in the relation.
            var expression = ParseLiteral(tokenizer);

            if (tokenizer.Current?.IsRelationalOperator() == true)
            {
                var @operator = tokenizer.Current.GetRelationalOperator();
                tokenizer.MoveNext(); // Consume operator.

                // Parse the right side literal in the relation.
                var right = ParseLiteral(tokenizer);

                return new RelationalExpression(
                    expression,
                    right,
                    @operator);
            }

            return expression;
        }

        private static Expression ParseLiteral(Tokenizer tokenizer)
        {
            var result = (Expression)null;
            if (tokenizer.Current != null)
            {
                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (tokenizer.Current.Type)
                {
                    case TokenType.Integer:
                        result = ParseConstant(tokenizer, value => int.Parse(value));
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
                        result = ParseScope(tokenizer);
                        break;
                    case TokenType.Word:
                        result = ParseProperty(tokenizer);
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

        private static Expression ParseProperty(Tokenizer tokenizer)
        {
            var name = tokenizer.Consume(TokenType.Word).Value;

            return new PropertyExpression(name);
        }

        private static Expression ParseConstant(Tokenizer tokenizer, Func<string, object> converter)
        {
            var expression = new ConstantExpression(converter(tokenizer.Current.Value));
            tokenizer.MoveNext(); // Move past constant.

            return expression;
        }

        private static Expression ParseScope(Tokenizer tokenizer)
        {
            tokenizer.Consume(TokenType.OpeningParenthesis);
            var expression = Parse(tokenizer);
            tokenizer.Consume(TokenType.ClosingParenthesis);

            return new ScopeExpression(expression);
        }
    }
}
