using Nyx.Utils;

namespace Nyx.Analysis;

public class EmptyNode : Node
{
    //TODO: check if this needs to be 0
    public EmptyNode(int pos) : base(new TextLocation(pos, 1)) { }
}
