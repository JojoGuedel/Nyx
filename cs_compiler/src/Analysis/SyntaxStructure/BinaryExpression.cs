using Nyx.Utils;

namespace Nyx.Analysis;

public class BinaryExpression : Expression
{
    public Expression left { get; }
    public LexerNode @operator { get; }
    public Expression right { get; }

    public BinaryExpression(Expression left, LexerNode opeartor, Expression right) : 
        base(TextLocation.Embrace(left.location, right.location))
    {
        this.left = left;
        this.@operator = opeartor;
        this.right = right;
    }

    public override void Write(TextWriter writer, string indent, bool isLast)
    {
        _WriteName(writer, indent, isLast, "BinaryExpression");
        indent += _ChildIndent(isLast);
        left.Write(writer, indent, false);
        @operator.Write(writer, indent, false);
        right.Write(writer, indent, true);
    }
}
