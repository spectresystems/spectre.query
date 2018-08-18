﻿namespace Spectre.Query.Internal.Expressions.Tokenization
{
    internal enum TokenType
    {
        Integer,
        Decimal,
        Word,
        String,
        EqualTo,
        NotEqualTo,
        GreaterThan,
        LessThan,
        GreaterThanOrEqualTo,
        LessThanOrEqualTo,
        OpeningParenthesis,
        ClosingParenthesis,
        Not,
        And,
        Or,
        True,
        False,
        Null,
    }
}
