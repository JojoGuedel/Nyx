using Nyx.Symbols;
using Nyx.Utils;

namespace Nyx.Analysis;

public class Postfix : Expression
{
    public Expression expression { get; }
    public SyntaxKind postfix { get; }

    public override TypeSymbol? typeSymbol => throw new NotImplementedException();

    public Postfix(Expression expression, LexerNode postfix) : 
        base(TextLocation.Embrace(expression.location, postfix.location))
    {
        this.expression = expression;
        this.postfix = postfix.kind;
    }
}
