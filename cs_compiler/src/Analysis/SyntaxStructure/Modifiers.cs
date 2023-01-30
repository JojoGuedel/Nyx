using Nyx.Utils;

namespace Nyx.Analysis;

public class Modifiers : Node 
{
    public Node global { get; }
    public Node @static { get; }
    public Node mutable { get; }

    public Modifiers(Node global, Node @static, Node mutableNode) :
        base(TextLocation.Embrace(global.location, mutableNode.location))
    {
        this.global = global;
        this.@static = @static;
        this.mutable = mutableNode;
    }

    public override void Write(TextWriter writer, string indent, bool isLast)
    {
        _WriteName(writer, indent, isLast, "Modifiers");
        indent += _ChildIndent(isLast);
        global.Write(writer, indent, false);
        @static.Write(writer, indent, false);
        mutable.Write(writer, indent, true);
    }
}
