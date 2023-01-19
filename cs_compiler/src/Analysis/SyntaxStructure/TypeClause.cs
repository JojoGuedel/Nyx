using System.Diagnostics;
using Nyx.Symbols;

namespace Nyx.Analysis;

public class  TypeClause : Node
{
    public string name { get; }

    // TODO: make constructor for arrow
    public TypeClause(LexerNode clauseInit, LexerNode name) :
        base(name.location)
    {
        Debug.Assert(name.value != null);

        this.name = name.value;
    }

    public TypeSymbol ConstructSymbol() => new TypeSymbol(name);
}
