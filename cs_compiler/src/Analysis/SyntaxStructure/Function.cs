using System.Collections.Immutable;
using Nyx.Utils;

namespace Nyx.Analysis;

public class Function : Member
{
    public Modifiers modifiers;
    public Expression name { get; }
    public ImmutableArray<Parameter> parameters { get; }
    public TypeClause type { get; }
    public Block body { get; }

    public Function
    (
        Modifiers modifiers, 
        LexerNode func, 
        Expression name,
        LexerNode lParen, 
        ImmutableArray<Parameter> parameters, 
        LexerNode rParen, 
        TypeClause type, 
        LexerNode colon, 
        Block body
    ) : base(TextLocation.Embrace(modifiers.location, body.location))
    {
        this.modifiers = modifiers;
        this.name = name;
        this.parameters = parameters;
        this.type = type;
        this.body = body;
    }

    public override void Write(TextWriter writer, string indent, bool isLast)
    {
        _WriteName(writer, indent, isLast, "Function");
        indent += _ChildIndent(isLast);
        modifiers.Write(writer, indent, false);
        name.Write(writer, indent, false);
        _WriteArray(writer, indent, false, "Parameter", Array.ConvertAll(parameters.ToArray(), (Parameter parameter) => (Node)parameter));
        type.Write(writer, indent, false);
        body.Write(writer, indent, true);
    }
}
