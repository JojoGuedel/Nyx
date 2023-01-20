namespace Nyx.Analysis;

class NodeWriter
{
    const string _indent = "    ";
    const string _childIndent = "│   ";
    const string _childNode = "├── ";
    const string _leafNode = "└── ";

    TextWriter _writer;

    public NodeWriter(TextWriter writer)
    {
        _writer = writer;
    }

    public void Write(Node nodes)
    {
        _writer.WriteLine($"{_leafNode}{nodes.kind}");

        var props = nodes.GetType().GetProperties();

        _Write(nodes);
    }

    public void Write(List<Node> nodes)
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            var isLast = i == nodes.Count - 1;
            _writer.WriteLine($"{(isLast? _leafNode : _childNode)}{nodes[i].kind}");

            if (nodes[i].children.Count != 0)
                _Write(nodes[i], $"{(isLast? _indent : _childIndent)}");
        }
    }

    void _Write(Node nodes, string indent = _indent)
    {
        for (int i = 0; i < nodes.children.Count; i++)
        {
            var isLast = i == nodes.children.Count - 1;
            _writer.WriteLine($"{indent}{(isLast? _leafNode : _childNode)}{nodes.children[i].kind}");

            if (nodes.children[i].children.Count != 0)
                _Write(nodes.children[i], $"{indent}{(isLast? _indent : _childIndent)}");
        }
    }
}