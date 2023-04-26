using Nyx.Utils;

namespace Nyx.Analysis;

public abstract class Expression : _Node
{
    protected Expression(TextLocation location) : base(location) { }
}
