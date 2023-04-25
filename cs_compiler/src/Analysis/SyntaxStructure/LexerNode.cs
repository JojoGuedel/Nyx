using Nyx.Utils;

namespace Nyx.Analysis;

public class LexerNode : _Node
{
    public SyntaxKind kind { get; }
    public string? value { get; }

    public LexerNode(SyntaxKind kind, TextLocation location, string? value = null) : 
        base(location)
    {
        this.kind = kind;
        this.value = value;
    }

    public override void Write(TextWriter writer, string indent, bool isLast)
    {
        _WriteName(writer, indent, isLast, "LexerNode");
        indent += _ChildIndent(isLast);
        _WriteKind(writer, indent, false, kind);
        _WriteString(writer, indent, true, value == null? "null" : value);
    }
}
