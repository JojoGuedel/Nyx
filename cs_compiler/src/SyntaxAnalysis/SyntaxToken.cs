using Utils;

namespace SyntaxAnalysis;

class SyntaxToken
{
    public SyntaxTokenKind Kind { get; }
    public TextLocation Location { get; }

    public SyntaxToken(SyntaxTokenKind kind, TextLocation location)
    {
        Kind = kind;
        Location = location;
    }

    static SyntaxTokenKind GetSingleToken(char a)
    {
        switch (a)
        {
            case '(': return SyntaxTokenKind.LPAREN;
            case ')': return SyntaxTokenKind.RPAREN;
            case '[': return SyntaxTokenKind.LSQUAR;
            case ']': return SyntaxTokenKind.RSQUAR;

            case '.': return SyntaxTokenKind.DOT;
            case ',': return SyntaxTokenKind.COMMA;
            case ';': return SyntaxTokenKind.SEMICOLON;

            case '+': return SyntaxTokenKind.PLUS;
            case '-': return SyntaxTokenKind.MINUS;
            case '*': return SyntaxTokenKind.STAR;
            case '/': return SyntaxTokenKind.SLASH;

            case '<': return SyntaxTokenKind.LESS;
            case '>': return SyntaxTokenKind.GREATER;
            case '=': return SyntaxTokenKind.EQUAL;
            case '%': return SyntaxTokenKind.PERCENT;
        }

        return SyntaxTokenKind.ERROR;
    }

    static SyntaxTokenKind GetDoubleToken(char c1, char c2)
    {
        switch (c1)
        {
            case '=':
                switch (c2)
                {
                    case '=': return SyntaxTokenKind.EQEQUALEQUAL;
                }
                break;
            case '!':
                switch (c2)
                {
                    case '=': return SyntaxTokenKind.NOTEQUAL;
                }
                break;
            case '<':
                switch (c2)
                {
                    case '=': return SyntaxTokenKind.LESSEQUAL;
                }
                break;
            case '>':
                switch (c2)
                {
                    case '=': return SyntaxTokenKind.GREATEREQUAL;
                }
                break;

            case '+':
                switch (c2)
                {
                    case '+': return SyntaxTokenKind.PLUSPLUS;
                    case '=': return SyntaxTokenKind.PLUSEQUAL;
                }
                break;
            case '-':
                switch (c2)
                {
                    case '-': return SyntaxTokenKind.MINUSMINUS;
                    case '=': return SyntaxTokenKind.MINEQUAL;
                    case '>': return SyntaxTokenKind.RARROW;
                }
                break;
            case '*':
                switch (c2)
                {
                    case '=': return SyntaxTokenKind.STAREQUAL;
                }
                break;
            case '/':
                switch (c2)
                {
                    case '=': return SyntaxTokenKind.SLASHEQUAL;
                }
                break;
            case '%':
                switch (c2)
                {
                    case '=': return SyntaxTokenKind.PERCENTEQUAL;
                }
                break;
        }

        return SyntaxTokenKind.ERROR;
    }
}
