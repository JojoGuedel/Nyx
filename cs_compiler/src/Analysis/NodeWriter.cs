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

    string _ComposeValue(object? value) => value is null? "[null]" : value.ToString()?? "[null]";

    void _WriteProperty(Node parent, PropertyInfo property, string indent, bool last, bool writeName)
    {
        object? value = property.GetValue(parent);

        if (value is null)
            return;

        if (value is Node node)
            _Write(node, indent, last);
        else if (typeof(IEnumerable<Node>).IsAssignableFrom(value.GetType()))
            _WriteEnumberable((IEnumerable<Node>)value, indent, last, property.Name);
        else
            _writer.WriteLine(
                _ComposeIndent(indent, last) +
                $"{property.Name} ({value.GetType().Name}): {_ComposeValue(value)}");
    }

    void _WriteEnumberable(IEnumerable<Node> nodes, string indent, bool last, string? name = null)
    {
        if (name is null)
            _writer.WriteLine(
                _ComposeIndent(indent, last) +
                nodes.GetType().Name + $"[{nodes.Count()}] ");
        else
            _writer.WriteLine(
                _ComposeIndent(indent, last) +
                name + $" ({nodes.GetType().Name}[{nodes.Count()}])");

        var length = nodes.Count();
        if (length == 0)
            return;

        indent = _ComposeChildIndent(indent, last);

        var index = 1;
        foreach(var node in nodes)
            _Write(node, indent, index++ >= length);
    }

    void _Write(Node node, string indent,  bool last) 
    {
        _writer.WriteLine(
            _ComposeIndent(indent, last) +
            node.GetType().Name);

        indent = _ComposeChildIndent(indent, last);

        var properites = node.GetType().GetProperties();

        if (properites.Length == 0)
            return;

        for(int i = 0; i < properites.Length - 1; i++)
            _WriteProperty(node, properites[i], indent, false, true);

        _WriteProperty(node, properites[properites.Length - 1], indent, true, true);
    }

    internal void Write(Node node) => _Write(node, "", true);
    internal void Write(ImmutableArray<Node> nodes) => _WriteEnumberable(nodes, "", true);
}