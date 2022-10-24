namespace SyntaxAnalysis;

enum SyntaxTokenKind
{
    ERROR,
    END,

    NAME,
    NUMBER,
    STRING,
    NEWLINE,
    INDENT,

    LPAREN,
    RPAREN,
    LSQUAR,
    RSQUAR,

    DOT,
    COMMA,
    SEMICOLON,

    PLUS,
    MINUS,
    STAR,
    SLASH,

    LESS,
    GREATER,
    EQUAL,
    PERCENT,

    LBRACE,
    RBRACE,
    EQEQUALEQUAL,
    NOTEQUAL,
    LESSEQUAL,
    GREATEREQUAL,

    PLUSPLUS,
    PLUSEQUAL,
    MINUSMINUS,
    MINEQUAL,
    STAREQUAL,
    SLASHEQUAL,
    PERCENTEQUAL,

    RARROW,
    OP,
}
