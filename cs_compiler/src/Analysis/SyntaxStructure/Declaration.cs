using System.Diagnostics;
using Nyx.Symbols;
using Nyx.Utils;

namespace Nyx.Analysis;

public class DeclarationStatement : Statement
{
    public Modifiers modifiers { get; }
    public Identifier name { get; }
    public TypeClause type { get; }
    public Expression assignment { get; }
    VariableSymbol ConstructSymbol() => throw new NotImplementedException();

    public DeclarationStatement(Modifiers modifiers, LexerNode var, Identifier name, TypeClause type, LexerNode equal, Expression expression, LexerNode semicolon) : 
        base(TextLocation.Embrace(modifiers.location, semicolon.location))
    {
        this.modifiers = modifiers;
        this.name = name;
        this.type = type;
        this.assignment = expression;
    }
}