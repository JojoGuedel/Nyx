using Nyx.Utils;

namespace Nyx.Analysis;

public class EmptyStatement : Statement
{
    //TODO: check if this needs to be 0
    public EmptyStatement(int pos) : base(new TextLocation(pos, 1)) { }

    public override void Write(TextWriter writer, string indent, bool isLast)
    {
        _WriteName(writer, indent, isLast, "EmptyStatement");
    }
}