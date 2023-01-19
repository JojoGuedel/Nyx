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
}
