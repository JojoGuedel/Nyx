using Nyx.Utils;

namespace Nyx.Analysis;

public class EmptyNode : _Node
{
    //TODO: check if this needs to be 0
    public EmptyNode(int pos) : base(new TextLocation(pos, 1)) { }

    public override void Write(TextWriter writer, string indent, bool isLast)
    {
        _WriteName(writer, indent, isLast, "EmptyNode");
    }
}

public class ErrorNode : LexerNode
{
    public LexerNode errorNode { get; }

    //TODO: check if this needs to be 0
    public ErrorNode(LexerNode node) : 
        base(SyntaxKind.Error, node.location) 
    { 
        errorNode = node;
    }
}
