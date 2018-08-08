using System;
using System.Collections.Generic;
using System.Text;

namespace Spectre.Query.Internal.Expressions.Tokenization
{
    internal sealed class Tokenizer
    {
        private readonly TextBuffer _buffer;
        private readonly Dictionary<string, (TokenType type, string representation)> _keywords;

        public int Position { get; private set; }
        public Token Current { get; private set; }

        public Tokenizer(string expression)
        {
            _buffer = new TextBuffer(expression);
            _keywords = new Dictionary<string, (TokenType, string)>(StringComparer.OrdinalIgnoreCase)
            {
                { "or", (TokenType.Or, "OR") },
                { "and", (TokenType.And, "AND") },
                { "not", (TokenType.Not, "NOT") },
                { "true", (TokenType.True, "true") },
                { "false", (TokenType.False, "false") },
                { "null", (TokenType.Null, "null") },
            };
        }

        public Token Consume(TokenType type)
        {
            var temp = Current;
            Expect(type);
            MoveNext();
            return temp;
        }

        public bool MoveNext()
        {
            EatWhitespace();

            Position = _buffer.Position;

            if (!_buffer.CanRead)
            {
                Current = null;
                return false;
            }

            var current = _buffer.Peek();
            var position = _buffer.Position;

            if (current == '\'' || current == '"')
            {
                Current = ReadStringLiteral(position);
            }
            else if (char.IsDigit(current))
            {
                Current = ReadInteger(position);
            }
            else if (char.IsLetter(current))
            {
                Current = ReadWord(position);
            }
            else
            {
                Current = ReadSymbol(position);
            }

            return true;
        }

        private void Expect(TokenType type)
        {
            if (Current?.Type != type)
            {
                throw new InvalidOperationException($"Expected token of type '{type}'.");
            }
        }

        private void EatWhitespace()
        {
            while (_buffer.CanRead)
            {
                var ch = _buffer.Peek();
                if (!char.IsWhiteSpace(ch))
                {
                    break;
                }
                _buffer.Read();
            }
        }

        private Token ReadInteger(int position)
        {
            var accumulator = new StringBuilder();
            while (_buffer.CanRead)
            {
                var i = _buffer.Peek();
                if (char.IsDigit(i))
                {
                    accumulator.Append(_buffer.Read());
                    continue;
                }
                break;
            }
            var value = accumulator.ToString();
            return new Token(TokenType.Integer, value, value, position);
        }

        private Token ReadStringLiteral(int position)
        {
            var accumulator = new StringBuilder();
            var start = _buffer.Read(); // Consume the '
            var character = _buffer.Peek();
            while (character != start)
            {
                accumulator.Append(character);
                _buffer.Read();
                if (!_buffer.CanRead)
                {
                    break;
                }
                character = _buffer.Peek();
            }
            if (character != start)
            {
                throw new InvalidOperationException("Unterminated string literal.");
            }
            _buffer.Read();

            var value = accumulator.ToString();
            return new Token(TokenType.String, value, string.Concat("'", value, "'"), position);
        }

        private Token ReadWord(int position)
        {
            var accumulator = new StringBuilder();
            var character = _buffer.Peek();
            while (char.IsLetter(character) || char.IsDigit(character))
            {
                _buffer.Read();
                accumulator.Append(character);
                if (!_buffer.CanRead)
                {
                    break;
                }
                character = _buffer.Peek();
            }

            var value = accumulator.ToString();

            // Is this a keyword?
            if (_keywords.TryGetValue(value, out var result))
            {
                return new Token(
                    result.type,
                    result.representation,
                    result.representation,
                    position);
            }

            return new Token(TokenType.Word, value, value, position);
        }

        private Token ReadSymbol(int position)
        {
            var character = _buffer.Read();
            var symbol = character.ToString();

            if (_buffer.CanRead)
            {
                var nextCharacter = _buffer.Peek();

                if (character == '!' && nextCharacter == '=')
                {
                    _buffer.Read();
                    return new Token(TokenType.NotEqualTo, "!=", "!=", position);
                }
                if (character == '<' && nextCharacter == '>')
                {
                    _buffer.Read();
                    return new Token(TokenType.NotEqualTo, "!=", "!=", position);
                }
                if (character == '&' && nextCharacter == '&')
                {
                    _buffer.Read();
                    return new Token(TokenType.And, "&&", "&&", position);
                }
                if (character == '|' && nextCharacter == '|')
                {
                    _buffer.Read();
                    return new Token(TokenType.Or, "||", "||", position);
                }
                if (character == '=' && nextCharacter == '=')
                {
                    _buffer.Read();
                    return new Token(TokenType.EqualTo, "==", "==", position);
                }
                if (character == '>' && nextCharacter == '=')
                {
                    _buffer.Read();
                    return new Token(TokenType.GreaterThanOrEqualTo, ">=", ">=", position);
                }
                if (character == '<' && nextCharacter == '=')
                {
                    _buffer.Read();
                    return new Token(TokenType.LessThanOrEqualTo, "<=", "<=", position);
                }
                if (character == '=')
                {
                    return new Token(TokenType.EqualTo, "==", "==", position);
                }
            }

            switch (character)
            {
                case '!':
                    return new Token(TokenType.Not, "!", "!", position);
                case '>':
                    return new Token(TokenType.GreaterThan, ">", ">", position);
                case '<':
                    return new Token(TokenType.LessThan, "<", "<", position);
                case '(':
                    return new Token(TokenType.OpeningParenthesis, "(", "(", position);
                case ')':
                    return new Token(TokenType.ClosingParenthesis, ")", ")", position);
                default:
                    throw new InvalidOperationException($"Encountered unexpected symbol '{symbol}'.");
            }
        }
    }
}
