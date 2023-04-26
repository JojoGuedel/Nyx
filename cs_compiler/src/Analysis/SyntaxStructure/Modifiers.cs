using Nyx.Utils;

namespace Nyx.Analysis;

public class Modifiers : _Node 
{
    public _Node @static { get; }
    public _Node mutable { get; }

    public Modifiers(_Node @static, _Node mutableNode) :
        base(TextLocation.Embrace(@static.location, mutableNode.location))
    {
        this.@static = @static;
        this.mutable = mutableNode;
    }

    public override void Write(TextWriter writer, string indent, bool isLast)
    {
        _WriteName(writer, indent, isLast, "Modifiers");
        indent += _ChildIndent(isLast);
        @static.Write(writer, indent, false);
        mutable.Write(writer, indent, true);
    }
}
