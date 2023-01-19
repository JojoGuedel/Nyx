using Nyx.Utils;

namespace Nyx.Analysis;

public abstract class Statement : Node
{
    protected Statement(TextLocation location) : base(location) { }
}