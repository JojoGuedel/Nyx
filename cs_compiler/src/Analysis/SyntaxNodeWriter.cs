namespace Nyx.Analysis;

class SytnaxNodeWriter
{
    private const string _indent = "    ";
    private const string _childIndent = "│   ";
    private const string _childNode = "├── ";
    private const string _leafNode = "└── ";


    private TextWriter _writer;

    public SytnaxNodeWriter(TextWriter writer)
    {
        _writer = writer;
    }

    public void Write(SyntaxNode syntaxNode)
    {
        _writer.WriteLine($"{_leafNode}{syntaxNode.kind}");

        _Write(syntaxNode);
    }

    public void Write(List<SyntaxNode> syntaxNodes)
    {
        for (int i = 0; i < syntaxNodes.Count; i++)
        {
            var isLast = i == syntaxNodes.Count - 1;
            _writer.WriteLine($"{(isLast? _leafNode : _childNode)}{syntaxNodes[i].kind}");

            if (syntaxNodes[i].children.Count != 0)
                _Write(syntaxNodes[i], $"{(isLast? _indent : _childIndent)}");
        }
    }

    private void _Write(SyntaxNode syntaxNode, string indent = _indent)
    {
        for (int i = 0; i < syntaxNode.children.Count; i++)
        {
            var isLast = i == syntaxNode.children.Count - 1;
            _writer.WriteLine($"{indent}{(isLast? _leafNode : _childNode)}{syntaxNode.children[i].kind}");

            if (syntaxNode.children[i].children.Count != 0)
                _Write(syntaxNode.children[i], $"{indent}{(isLast? _indent : _childIndent)}");
        }
    }
}