using System.Collections.Immutable;
using Nyx.Symbols;
using Nyx.Utils;

namespace Nyx.Analysis;

public class Function : Node
{
    public Modifiers modifiers;
    public ImmutableArray<Parameter> parameters { get; }
    public TypeClause type { get; }
    public Block body { get; }

    public Function(Modifiers modifiers, ImmutableArray<Parameter> parameters, TypeClause type, Block body) :
        base(TextLocation.Embrace(modifiers.location, body.location))
    {
        this.modifiers = modifiers;
        this.parameters = parameters;
        this.type = type;
        this.body = body;
    }
}

public class FunctionCall : Expression
{
    public LexerNode name { get; }

    public override TypeSymbol? typeSymbol => throw new NotImplementedException();

    public ImmutableArray<Expression> arguments;
    // TODO: optional arguments
    
    public FunctionCall(LexerNode name, ImmutableArray<Expression> arguments, TextLocation location) : 
        base(location)
    {
        this.name = name;
        this.arguments = arguments;
    }
}