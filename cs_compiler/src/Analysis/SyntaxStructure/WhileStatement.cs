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
}
