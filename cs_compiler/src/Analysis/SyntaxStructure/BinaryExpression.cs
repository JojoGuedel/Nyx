namespace Nyx.Analysis;

internal class BinaryExpression : Expression
{
    internal override Location location { get; }
    public Expression left { get; }
    public Token @operator { get; }
    public Expression right { get; }

    internal BinaryExpression(Expression left, Token opeartor, Expression right)
    {
        location = Location.Embrace(left.location, right.location);
        this.left = left;
        this.@operator = opeartor;
        this.right = right;
    }
}
