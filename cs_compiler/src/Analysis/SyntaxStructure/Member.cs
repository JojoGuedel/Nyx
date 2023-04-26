using Nyx.Utils;

namespace Nyx.Analysis;

internal abstract class Member : Node { }

public abstract class _Member : _Node
{
    protected _Member(TextLocation location) : base(location) { }
}
