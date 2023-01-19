using Nyx.Utils;

namespace Nyx.Analysis;

public class LexerNode : Node
{
    public SyntaxKind kind { get; }
    public string? value { get; }

    public LexerNode(SyntaxKind kind, TextLocation location, string? value = null) : 
        base(location)
    {
        this.kind = kind;
        this.value = value;
    }
}