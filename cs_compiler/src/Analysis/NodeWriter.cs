using System.Collections.Immutable;

namespace Nyx.Analysis;

public class NodeWriter
{
    TextWriter _writer;

    public NodeWriter(TextWriter writer)
    {
        _writer = writer;
    }

    public void Write(_Node node)
    {
        node.Write(_writer, "", true);
    }

    public void Write(ImmutableArray<_Node> nodes)
    {
        for(int i = 0; i < nodes.Length; i++)
        {
            var isLast = i == nodes.Length - 1;
            nodes[i].Write(_writer, "", isLast);
        }
    }
}