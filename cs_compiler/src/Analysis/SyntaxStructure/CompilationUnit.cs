using System.Collections.Immutable;
using Nyx.Utils;

namespace Nyx.Analysis;

internal class CompilationUnit : Node
{
    internal override Location location { get; }
    internal ImmutableArray<Member> members { get; }

    internal CompilationUnit(ImmutableArray<Member> members, Token end)
    {
        this.members = members;

        if (members.Length > 0)
            location = Location.Embrace(members[0].location, end.location);
        else
            location = end.location;
    }
}

public class _CompilationUnit : _Node
{
    public ImmutableArray<_Member> members { get; }

    public _CompilationUnit(ImmutableArray<_Member> members, LexerNode end) : 
        base(members.Length > 0? TextLocation.Embrace(members[0].location, end.location) : end.location)
    {
        this.members = members;
    }

    public override void Write(TextWriter writer, string indent, bool isLast)
    {
        _WriteName(writer, indent, isLast, "CompilationUnit");
        indent += _ChildIndent(isLast);
        _WriteArray(writer, indent, true, "Member", Array.ConvertAll(members.ToArray(), (_Member member) => (_Node)member));
    }
}