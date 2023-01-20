using Nyx.Utils;

namespace Nyx.Analysis;

public abstract class Member : Node
{
    protected Member(TextLocation location) : base(location) { }
}