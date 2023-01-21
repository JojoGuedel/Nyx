using System.Collections.Immutable;

namespace Nyx.Analysis;

public static class NodeWriterIndents
{
    public const string indent = "    ";
    public const string childIndent = "│   ";
    public const string childNode = "├── ";
    public const string leafNode = "└── ";
}

public class NodeWriter
{
    TextWriter _writer;

    public NodeWriter(TextWriter writer)
    {
        _writer = writer;
    }

    public void Write(Node node)
    {
        node.Write(_writer, "", true);
    }

    public void Write(ImmutableArray<Node> nodes)
    {
        for(int i = 0; i < nodes.Length; i++)
        {
            var isLast = i == nodes.Length - 1;
            nodes[i].Write(_writer, "", isLast);
        }
    }
}