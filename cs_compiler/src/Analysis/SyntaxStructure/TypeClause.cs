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

    public override void Write(TextWriter writer, string indent, bool isLast)
    {
        _WriteName(writer, indent, isLast, "TypeClause");
        indent += _ChildIndent(isLast);
        type.Write(writer, indent, true);
    }
}
