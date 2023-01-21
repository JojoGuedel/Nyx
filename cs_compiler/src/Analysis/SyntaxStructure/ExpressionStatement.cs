using System.Diagnostics;
using Nyx.Utils;

namespace Nyx.Analysis;

public class ExpressionStatement : Statement
{
    Expression expression;

    public ExpressionStatement(Expression expression, LexerNode semicolon) : 
        base(TextLocation.Embrace(expression.location, semicolon.location))
    {
        this.expression = expression;
    }

    public override void Write(TextWriter writer, string indent, bool isLast)
    {
        _WriteName(writer, indent, isLast, "ExpressionStatement");
        indent += _ChildIndent(isLast);
        expression.Write(writer, indent, true);
    }
}