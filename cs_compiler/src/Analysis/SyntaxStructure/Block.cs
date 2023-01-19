using System.Collections.Immutable;
using Nyx.Utils;

namespace Nyx.Analysis;

public class Block : Node
{
    public ImmutableArray<Statement> statements;

    public Block(ImmutableArray<Statement> statements, TextLocation location) : 
        base(location)
    {
        this.statements = statements;
    }
}
