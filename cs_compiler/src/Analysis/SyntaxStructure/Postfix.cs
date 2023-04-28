using Nyx.Utils;

namespace Nyx.Analysis;

public class Postfix : _Expression
{
    public _Expression expression { get; }
    public LexerNode postfix { get; }

    public Postfix(_Expression expression, LexerNode postfix) : 
        base(TextLocation.Embrace(expression.location, postfix.location))
    {
        this.expression = expression;
        this.postfix = postfix;
    }

    public override void Write(TextWriter writer, string indent, bool isLast)
    {
        _WriteName(writer, indent, isLast, "Postfix");
        indent += _ChildIndent(isLast);
        expression.Write(writer, indent, false);
        postfix.Write(writer, indent, true);
    }
}
