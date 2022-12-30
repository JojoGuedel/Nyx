namespace Nyx.CodeAnalysis;

public class IndentAnalyzer : Analyzer<SyntaxNode, SyntaxNode>
{
    public IndentAnalyzer(List<SyntaxNode> values, SyntaxNode terminator) : base(values, terminator)
    {
    }

    public override IEnumerable<SyntaxNode> GetAll()
    {
        throw new NotImplementedException();
    }
}