using Utils;

class SyntaxToken
{
    public SyntaxKind Kind { get; }
    public TextSpan Span { get; }

    public SyntaxToken(SyntaxKind kind, TextSpan span) 
    {
        Kind = kind;
        Span = span;
    }
}
