using System;
using System.Collections.Generic;

namespace Spectre.Query.Internal.Expressions.Tokenization
{
    internal static class TokenExtensions
    {
        private static readonly Dictionary<TokenType, RelationalOperator> Operators;

        static TokenExtensions()
        {
            Operators = new Dictionary<TokenType, RelationalOperator>
            {
                { TokenType.EqualTo, RelationalOperator.EqualTo },
                { TokenType.NotEqualTo, RelationalOperator.NotEqualTo },
                { TokenType.GreaterThan, RelationalOperator.GreaterThan },
                { TokenType.GreaterThanOrEqualTo, RelationalOperator.GreaterThanOrEqualTo },
                { TokenType.LessThan, RelationalOperator.LessThan },
                { TokenType.LessThanOrEqualTo, RelationalOperator.LessThanOrEqualTo }
            };
        }

        public static bool IsRelationalOperator(this Token token)
        {
            return Operators.ContainsKey(token.Type);
        }

        public static RelationalOperator GetRelationalOperator(this Token token)
        {
            if (Operators.ContainsKey(token.Type))
            {
                return Operators[token.Type];
            }
            throw new InvalidOperationException("Unknown relational operator.");
        }
}
}
