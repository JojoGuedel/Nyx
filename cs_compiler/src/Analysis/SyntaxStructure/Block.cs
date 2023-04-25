using System.Collections.Immutable;
using Nyx.Utils;

namespace Nyx.Analysis;

public class Block : _Node
{
    public ImmutableArray<Statement> statements;

    public Block(ImmutableArray<Statement> statements, TextLocation location) : 
        base(location)
    {
        this.statements = statements;
    }

    public override void Write(TextWriter writer, string indent, bool isLast)
    {
        _WriteName(writer, indent, isLast, "Block");
        indent += _ChildIndent(isLast);
        _WriteArray(writer, indent, true, "Statement", Array.ConvertAll(statements.ToArray(), (Statement statement) => (_Node)statement));
    }
}
