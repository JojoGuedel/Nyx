using Utils;

namespace SyntaxAnalysis;

public class SyntaxToken
{
    public SyntaxTokenKind kind { get; }
    public TextLocation location { get; }

    public SyntaxToken(SyntaxTokenKind kind, TextLocation location)
    {
        this.kind = kind;
        this.location = location;
    }
}
