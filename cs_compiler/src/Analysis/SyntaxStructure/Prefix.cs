namespace Nyx.Analysis;

internal class Prefix : Expression
{
    internal override Location location => Location.Embrace(prefix.location, expression.location);
    public Token prefix { get; }
    public Expression expression { get; }

    internal Prefix(Token prefix, Expression expression)
    {
        this.prefix = prefix;
        this.expression = expression;
    }
}