using System.Collections.Immutable;
using Nyx.Utils;

namespace Nyx.Analysis;

public class GlobalFunction : GlobalMember
{
    public Identifier name { get; }
    public ImmutableArray<Parameter> parameters { get; }
    public TypeClause type { get; }
    public Block body { get; }

    public GlobalFunction
    (
        LexerNode global, 
        LexerNode func, 
        Identifier name,
        LexerNode lParen, 
        ImmutableArray<Parameter> parameters, 
        LexerNode rParen,
        TypeClause type, 
        LexerNode colon, 
        Block body
    ) : base(TextLocation.Embrace(global.location, body.location))
    {
        this.name = name;
        this.parameters = parameters;
        this.type = type;
        this.body = body;
    }

    public override void Write(TextWriter writer, string indent, bool isLast)
    {
        _WriteName(writer, indent, isLast, "GlobalFunction");
        indent += _ChildIndent(isLast);
        name.Write(writer, indent, false);
        _WriteArray(writer, indent, false, "Parameter", Array.ConvertAll(parameters.ToArray(), (Parameter parameter) => (Node)parameter));
        type.Write(writer, indent, false);
        body.Write(writer, indent, true);
    }
}