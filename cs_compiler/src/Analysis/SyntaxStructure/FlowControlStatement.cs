using System.Collections.Immutable;
using Nyx.Utils;

namespace Nyx.Analysis;

public class FlowControlStatement : Statement
{
    public ImmutableArray<LexerNode> statements { get; }

    public FlowControlStatement(ImmutableArray<LexerNode> statements, LexerNode semicolon) : 
        base(TextLocation.Embrace(statements[0].location, semicolon.location))
    {
        this.statements = statements;
    }

    public override void Write(TextWriter writer, string indent, bool isLast)
    {
        _WriteName(writer, indent, isLast, "FlowControlStatement");
        indent += _ChildIndent(isLast);
        _WriteArray(writer, indent, true, "LexerNode", Array.ConvertAll(statements.ToArray(), (LexerNode statement) => (Node)statement));
    }
}


