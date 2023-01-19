using System.Diagnostics;
using Nyx.Utils;

namespace Nyx.Analysis;

public class IfStatement : Statement
{
    public Expression condition { get; }
    public Block body { get; }
    public Statement else_ { get; }

    public IfStatement(Expression condition, Block body, Statement else_) : 
        base(TextLocation.Embrace(condition.location, else_.location))
    {
        Debug.Assert(condition is Expression);
        Debug.Assert(else_ is Statement);

        this.condition = condition;
        this.body = body;
        this.else_ = else_;
    }
}
