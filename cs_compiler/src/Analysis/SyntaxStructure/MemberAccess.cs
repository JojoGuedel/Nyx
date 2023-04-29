namespace Nyx.Analysis;

internal class MemberAccess : Expression
{
    internal override Location location { get; }
    internal Expression expression { get; }
    internal Identifier identifier { get; }

    internal MemberAccess(Expression expression, Token dot, ValueToken name)
    {
        location = Location.Embrace(expression.location, name.location);
        this.expression = expression;
        this.identifier = new Identifier(name);
    }
}