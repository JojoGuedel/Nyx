using System.Diagnostics;
using Nyx.Symbols;
using Nyx.Utils;

namespace Nyx.Analysis;

public class Parameter : Node
{
    public Modifiers modifiers { get; }
    public string name { get; }
    public TypeClause type { get; }

    public Parameter(Modifiers modifiers, LexerNode var, LexerNode name, TypeClause type) 
        : base(TextLocation.Embrace(modifiers.location, type.location))
    {
        Debug.Assert(name.value != null);

        this.modifiers = modifiers;
        this.name = name.value;
        this.type = type;
    }

    public override void Write(TextWriter writer, string indent, bool isLast)
    {
        _WriteName(writer, indent, isLast, "Parameter");
        indent += _ChildIndent(isLast);
        modifiers.Write(writer, indent, false);
        _WriteString(writer, indent, false, name);
        type.Write(writer, indent, true);
    }
}
