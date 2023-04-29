using System.Collections.Immutable;

namespace Nyx.Analysis;

internal class CompilationUnit : Node
{
    internal override Location location { get; }
    public ImmutableArray<Member> members { get; }

    internal CompilationUnit(ImmutableArray<Member> members, Token end)
    {
        if (members.Length > 0)
            location = Location.Embrace(members[0].location, end.location);
        else
            location = end.location;
        this.members = members;
    }
}