using System.Diagnostics;
using Nyx.Symbols;
using Nyx.Utils;

namespace Nyx.Analysis;

public class DeclarationStatement : Statement
{
    public Modifiers modifiers { get; }
    public string name { get; }
    public TypeClause type { get; }
    public Expression assignment { get; }

    public DeclarationStatement(Modifiers modifiers, LexerNode var, LexerNode name, TypeClause type, LexerNode equal, Expression expression, LexerNode semicolon) : 
        base(TextLocation.Embrace(modifiers.location, semicolon.location))
    {
        Debug.Assert(name.value != null);

        this.modifiers = modifiers;
        this.name = name.value;
        this.type = type;
        this.assignment = expression;
    }

    VariableSymbol ConstructSymbol() => new VariableSymbol(name, modifiers.ConstructSymbol(), type.ConstructSymbol());
}