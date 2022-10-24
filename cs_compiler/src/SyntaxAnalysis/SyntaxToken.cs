using Utils;

namespace SyntaxAnalysis;

class SyntaxToken
{
    public SyntaxTokenKind Kind { get; }
    public TextSpan Span { get; }

    public SyntaxToken(SyntaxTokenKind kind, TextSpan span) 
    {
        Kind = kind;
        Span = span;
    }
}
