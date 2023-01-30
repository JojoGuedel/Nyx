using System.Diagnostics;

namespace Nyx.Analysis;

public class Identifier : Expression
{
    public string name { get; }

    public Identifier(LexerNode name) : base(name.location)
    {
        Debug.Assert(name.value != null);
        this.name = name.value;
    }

    public override void Write(TextWriter writer, string indent, bool isLast)
    {
        _WriteName(writer, indent, isLast, "Identifier");
        indent += _ChildIndent(isLast);
        _WriteString(writer, indent, true, name);
    }
}