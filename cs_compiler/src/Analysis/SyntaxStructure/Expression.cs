using Nyx.Utils;

namespace Nyx.Analysis;

public abstract class Expression : Node
{
    protected Expression(TextLocation location) : base(location) { }
}
