namespace SyntaxAnalysis;

public enum SyntaxTokenKind
{
    ERROR,
    END,
    DISCARD,

    INDENT,
    NEWLINE,
    NUMBER,
    STRING,
    NAME,
    UNUSEDNAME,

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

    // LBRACE,
    // RBRACE,
    EQUALEQUAL,
    NOTEQUAL,
    LESSEQUAL,
    GREATEREQUAL,

    PLUSPLUS,
    PLUSEQUAL,
    MINUSMINUS,
    MINUSEQUAL,
    STAREQUAL,
    SLASHEQUAL,
    PERCENTEQUAL,

    RARROW,
    OP,
}
