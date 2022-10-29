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
}
