namespace Nyx.Analysis;

internal class Postfix : Expression
{
    internal override Location location => Location.Embrace(expression.location, postfix.location);
    public Expression expression { get; }
    public Token postfix { get; }

    internal Postfix(Expression expression, Token postfix)
    {
        this.expression = expression;
        this.postfix = postfix;
    }
}
