using Nyx.Symbols;
using Nyx.Utils;

namespace Nyx.Analysis;

public class Modifiers : Node 
{
    public Node @static { get; }
    public Node mutable { get; }

    public Modifiers(Node @static, Node mutableNode) :
        base(TextLocation.Embrace(@static.location, mutableNode.location))
    {
        this.@static = @static;
        this.mutable = mutableNode;
    }
    
    public SymbolModifiers ConstructSymbol() => new SymbolModifiers
    (
        !(@static is EmptyNode),
        !(mutable is EmptyNode)
    );

    public override void Write(TextWriter writer, string indent, bool isLast)
    {
        _WriteName(writer, indent, isLast, "Modifiers");
        indent += _ChildIndent(isLast);
        @static.Write(writer, indent, false);
        mutable.Write(writer, indent, true);
    }
}
