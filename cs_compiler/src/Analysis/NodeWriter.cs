using System.Collections.Immutable;
using System.Reflection;

namespace Nyx.Analysis;

internal class NodeWriter
{
    internal const string emptyIndent = "    ";
    internal const string childIndent = "│   ";
    internal const string childNodeIndent = "├── ";
    internal const string leafNodeIndent = "└── ";

    TextWriter _writer;

    internal NodeWriter(TextWriter writer)
    {
        _writer = writer;
    }

    string _ComposeChildIndent(string indent, bool last) => indent + (last? emptyIndent : childIndent);

    string _ComposeIndent(string indent, bool last) => indent + (last? leafNodeIndent : childNodeIndent);

    string _ComposeName(Type type) => type.Name;

    string _ComposeValue(object? value) => value is null? "<null>" : value.ToString()?? "<null>";

    string _Compose(string indent, string name, string? value = null) => indent + name + (value is null? "" : ": " + value);

    void _WriteProperty(Node parent, PropertyInfo property, string indent, bool last)
    {
        object? value = property.GetValue(parent);

        if (value is null)
            return;

        switch (value)
        {
            case Node node:
                _Write(node, indent, false);
                break;
            case ImmutableArray<Node> nodes:
                _Write(nodes, indent, false);
                break;
            default:
                _writer.WriteLine(_Compose(_ComposeIndent(indent, last), _ComposeName(value.GetType()), _ComposeValue(value)));
                break;
        } 
    }

    void _Write(Node node, string indent,  bool last) 
    {
        _writer.WriteLine(_Compose(_ComposeIndent(indent, last), _ComposeName(node.GetType())));
        indent = _ComposeChildIndent(indent, last);

        var properites = node.GetType().GetProperties();

        if (properites.Length == 0)
            return;

        for(int i = 0; i < properites.Length - 1; i++)
            _WriteProperty(node, properites[i], indent, false);

        _WriteProperty(node, properites[properites.Length - 1], indent, true);
    }

    void _Write(ImmutableArray<Node> nodes, string indent, bool last)
    {
        _writer.WriteLine(_Compose(_ComposeIndent(indent, last), _ComposeName(nodes.GetType())));
        
        indent = _ComposeChildIndent(indent, last);

        for(int i = 0; i < nodes.Length - 1; i++)
            _Write(nodes[i], indent, false);

        _Write(nodes[nodes.Length - 1], indent, true);
    }

    internal void Write(Node node) => _Write(node, "", true);
    internal void Write(ImmutableArray<Node> nodes) => _Write(nodes, "", true);
}