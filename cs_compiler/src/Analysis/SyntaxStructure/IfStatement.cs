using System.Diagnostics;
using Nyx.Utils;

namespace Nyx.Analysis;

public class IfStatement : Statement
{
    public Expression condition { get; }
    public Block body { get; }
    public Statement @else { get; }

    public IfStatement(LexerNode @if, Expression condition, Block body, Statement @else) : 
        base(TextLocation.Embrace(@if.location, @else.location))
    {
        this.condition = condition;
        this.body = body;
        this.@else = @else;
    }

    public override void Write(TextWriter writer, string indent, bool isLast)
    {
        _WriteName(writer, indent, isLast, "IfStatement");
        indent += _ChildIndent(isLast);
        condition.Write(writer, indent, false);
        body.Write(writer, indent, false);
        @else.Write(writer, indent, true);
    }
}