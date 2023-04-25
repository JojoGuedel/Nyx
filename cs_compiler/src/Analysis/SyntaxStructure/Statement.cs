using Nyx.Utils;

namespace Nyx.Analysis;

public abstract class Statement : _Node
{
    protected Statement(TextLocation location) : base(location) { }
}