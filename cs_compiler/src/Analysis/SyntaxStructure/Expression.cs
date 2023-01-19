using Nyx.Symbols;
using Nyx.Utils;

namespace Nyx.Analysis;

public abstract class Expression : Node
{
    public abstract TypeSymbol? typeSymbol { get; }

    protected Expression(TextLocation location) : base(location) { }
}
