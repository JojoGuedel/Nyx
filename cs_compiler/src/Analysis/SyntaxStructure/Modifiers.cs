using Nyx.Symbols;
using Nyx.Utils;

namespace Nyx.Analysis;

public class Modifiers : Node 
{
    public Node staticNode { get; }
    public Node mutableNode { get; }

    public Modifiers(Node staticNode, Node mutableNode) :
        base(TextLocation.Embrace(staticNode.location, mutableNode.location))
    {
        this.staticNode = staticNode;
        this.mutableNode = mutableNode;
    }
    
    public SymbolModifiers ConstructSymbol() => new SymbolModifiers
    (
        !(staticNode is EmptyNode),
        !(mutableNode is EmptyNode)
    );
}
