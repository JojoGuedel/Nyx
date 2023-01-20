using System.Collections.Immutable;

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

    public void Write(Node node)
    {
        _writer.WriteLine($"{_leafNode}{node.GetType().Name}");
        _Write(node);
    }

    public void Write(List<Node> nodes)
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            var isLast = i == nodes.Count - 1;
            _writer.WriteLine($"{(isLast? _leafNode : _childNode)}{nodes[i].GetType().Name}");
            _Write(nodes[i], $"{(isLast? _indent : _childIndent)}");
        }
    }

    void _Write(Node node, string indent = _indent)
    {
        var props = node.GetType().GetProperties();

        for (int i = 0; i < props.Length; i++)
        {
            var isLast = i == props.Length - 1;
            var prop = props[i];

            if (prop.PropertyType.GetInterface(typeof(IEnumerable<>).FullName) != null)
            {
                _writer.WriteLine($"{indent}{(isLast? _leafNode : _childNode)}{prop.Name}[]");

                var val = (IEnumerable<object>?)prop.GetValue(node);
                if (val == null) continue;
                var arr = val.ToImmutableArray();

                for(var j = 0; j < arr.Length; j++) 
                {
                    if (arr[j] is not Node) break;
                    // var _isLast = j == arr.Length - 1;
                    _Write((Node)arr[i], $"{indent}{(isLast? _indent : _childIndent)}");
                }

            }
            if (prop.GetType() != typeof(Node))
            {
                _writer.WriteLine($"{indent}{(isLast? _leafNode : _childNode)}{prop.Name}: {prop.GetValue(node)}");
                return;
            }

            var child = (Node?)prop.GetValue(null);
            
            if (child is not null)
                _Write(child, $"{indent}{(isLast? _indent : _childIndent)}");
        }
    }
}