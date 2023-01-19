using System.Collections.Immutable;
using Nyx.Utils;

namespace Nyx.Analysis;

public class Function : Node
{
    public Modifiers modifiers;
    public ImmutableArray<Parameter> parameters { get; }
    public TypeClause type { get; }
    public Block body { get; }

    public Function(Modifiers modifiers, LexerNode lParen, ImmutableArray<Parameter> parameters, LexerNode rParen, TypeClause type, Block body) :
        base(TextLocation.Embrace(modifiers.location, body.location))
    {
        this.modifiers = modifiers;
        this.parameters = parameters;
        this.type = type;
        this.body = body;
    }
}
