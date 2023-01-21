using Nyx.Utils;

namespace Nyx.Analysis;

public class WhileStatement : Statement
{
    public Expression condition { get; }
    public Block body { get; }

    public WhileStatement(LexerNode @while, Expression condition, Block body) :
        base(TextLocation.Embrace(@while.location, body.location))
    {
        this.condition = condition;
        this.body = body;
    }

    public override void Write(TextWriter writer, string indent, bool isLast)
    {
        _WriteName(writer, indent, isLast, "FunctionCall");
        indent += _ChildIndent(isLast);
        condition.Write(writer, indent, false);
        body.Write(writer, indent, true);
    }
}
