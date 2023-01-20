using System.Diagnostics;
using Nyx.Symbols;

namespace Nyx.Analysis;

public class  TypeClause : Node
{
    public Expression type { get; }
    public TypeSymbol ConstructSymbol() => throw new NotImplementedException();

    // TODO: make constructor for arrow
    public TypeClause(LexerNode clauseInit, Expression type) :
        base(type.location)
    {
        this.type = type;
    }
}
