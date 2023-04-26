namespace Nyx.Analysis;

public class  TypeClause : _Node
{
    public Expression type { get; }

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

    // public bool TryAssignSymbol(TypeSystem.Symbol typeSymbol)
    // {
    //     if (this.symbol is not null)
    //         return false;
        
    //     symbol = typeSymbol;
    //     return true;
    // }
}