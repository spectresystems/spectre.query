namespace Spectre.Query.Internal.Expressions.Tokenization
{
    internal sealed class Token
    {
        public TokenType Type { get; }
        public string Value { get; }
        public string Representation { get; }
        public int Position { get; }

        public Token(TokenType type, string value, string representation, int position)
        {
            Type = type;
            Value = value;
            Representation = representation;
            Position = position;
        }
    }
}
