using Nyx.Utils;

namespace Nyx.Analysis;

public class ReturnStatement : Statement
{
    public Expression expression { get; }

    public ReturnStatement(LexerNode @return, Expression expression, LexerNode semicolon) : 
        base(TextLocation.Embrace(@return.location, semicolon.location))
    {
        this.expression = expression;
    }

    public override void Write(TextWriter writer, string indent, bool isLast)
    {
        _WriteName(writer, indent, isLast, "ReturnStatement");
        indent += _ChildIndent(isLast);
        expression.Write(writer, indent, true);
    }
}