using Nyx.Utils;

namespace Nyx.Analysis;

public abstract class GlobalMember : _Member
{
    protected GlobalMember(TextLocation location) : base(location) { }
}