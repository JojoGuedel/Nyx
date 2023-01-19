using Nyx.Utils;

namespace Nyx.Analysis;

public abstract class Node
{
    public TextLocation location { get; }

    public Node(TextLocation location)
    {
        this.location = location;
    }
}