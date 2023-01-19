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
}
