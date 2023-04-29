using System.Collections.Immutable;
using System.Diagnostics;

namespace Nyx.Analysis;

internal class FlowControlStatement : Statement
{
    internal override Location location { get; }
    public ImmutableArray<Token> statements { get; }

    internal FlowControlStatement(ImmutableArray<Token> statements, Token semicolon, Token newLine)
    {
        Debug.Assert(statements.Length > 0);
        location = Location.Embrace(statements[0].location, semicolon.location);
        this.statements = statements;
    }
}


