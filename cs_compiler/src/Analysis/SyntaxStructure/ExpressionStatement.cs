namespace Nyx.Analysis;

internal class ExpressionStatement : Statement
{
    internal override Location location { get; }
    public Expression expression { get; }

    internal ExpressionStatement(Expression expression, Token semicolon, Token newLine)
    {
        location = Location.Embrace(expression.location, semicolon.location);
        this.expression = expression;
    }
}