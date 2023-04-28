using System.Collections.Immutable;

namespace Nyx.Analysis;

internal class Function : Member
{
    internal override Location location { get; }
    public Modifiers modifiers;
    public Expression name { get; }
    public ImmutableArray<Parameter> parameters { get; }
    public TypeClause type { get; }
    public Block body { get; }

    internal Function(
        Modifiers modifiers,
        Token func,
        Expression name,
        Token lParen,
        ImmutableArray<Parameter> parameters,
        Token rParen,
        TypeClause type,
        Token colon,
        Token newLine,
        Block body)
    {
        location = Location.Embrace(modifiers.location, body.location);

        this.modifiers = modifiers;
        this.name = name;
        this.parameters = parameters;
        this.type = type;
        this.body = body;
    }
}