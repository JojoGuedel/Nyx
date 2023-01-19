using System.Diagnostics;
using Nyx.Symbols;
using Nyx.Utils;

namespace Nyx.Analysis;

public class BinaryExpression : Expression
{
    public Expression left { get; }
    public LexerNode operator_ { get; }
    public Expression right { get; }

    public override TypeSymbol? typeSymbol => throw new NotImplementedException();

    public BinaryExpression(Expression left, LexerNode opeartor_, Expression right) : 
        base(TextLocation.Embrace(left.location, right.location))
    {
        this.left = left;
        this.operator_ = opeartor_;
        this.right = right;
    }
}
