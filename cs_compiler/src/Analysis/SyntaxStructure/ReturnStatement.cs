namespace Nyx.Analysis;

internal class ReturnStatement : Statement
{
    internal override Location location { get; }
    public Expression expression { get; }

    internal ReturnStatement(Token @return, Expression expression, Token semicolon, Token newLine)
    {
        location = Location.Embrace(@return.location, semicolon.location);
        this.expression = expression;
    }
}