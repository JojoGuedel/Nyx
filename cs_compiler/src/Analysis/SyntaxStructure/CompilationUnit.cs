using System.Collections.Immutable;
using Nyx.Utils;

namespace Nyx.Analysis;

public class CompilationUnit : Node
{
    public ImmutableArray<Member> members { get; }

    public CompilationUnit(ImmutableArray<Member> members, LexerNode end) : 
        base(members.Length > 0? TextLocation.Embrace(members[0].location, end.location) : end.location)
    {
        this.members = members;
    }

    public override void Write(TextWriter writer, string indent, bool isLast)
    {
        _WriteName(writer, indent, isLast, "CompilationUnit");
        indent += _ChildIndent(isLast);
        _WriteArray(writer, indent, true, "Member", Array.ConvertAll(members.ToArray(), (Member member) => (Node)member));
    }
}