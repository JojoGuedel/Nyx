namespace SyntaxAnalysis;

enum SyntaxTokenKind
{
    UndefinedPattern,
    InvalidChar,
    End,

    Plus,
    Minus,
    Star,
    Slash,
    Equal,
    PlusEqual,
    MinusEqual,
    StarEqual,
    SlashEqual,
    EqualEqual,
    NotEqual,
    LessEqual,
    GreaterEqual,
    Less,
    Greater,

    LeftParen,
    RightParen,
    LeftSquare,
    RightSquare,
    LeftCurly,
    RightCurly,

    Int,
}
