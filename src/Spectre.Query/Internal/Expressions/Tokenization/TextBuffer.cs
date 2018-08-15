using System;

namespace Spectre.Query.Internal.Expressions.Tokenization
{
    internal sealed class TextBuffer
    {
        private readonly int _length;
        private readonly string _text;
        private int _position;

        public int Position => _position;
        public bool CanRead => Peek() != char.MaxValue;

        public TextBuffer(string text)
        {
            _length = text.Length;
            _text = text;
        }

        public char Peek()
        {
            return _position >= _length
                ? char.MaxValue
                : _text[_position];
        }

        public char Read()
        {
            if (_position >= _length)
            {
                return char.MaxValue;
            }
            var result = _text[_position];
            _position++;
            return result;
        }

        public char Consume(char expected)
        {
            var current = Read();
            if (current != expected)
            {
                throw new InvalidOperationException($"Expected '{expected}' but encountered '{current}'.");
            }
            return current;
        }
    }
}
