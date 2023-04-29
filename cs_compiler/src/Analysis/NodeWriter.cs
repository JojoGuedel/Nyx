using System.Collections.Immutable;
using System.Reflection;

namespace Nyx.Analysis;

internal class NodeWriter
{
    internal const string emptyIndent = "  ";
    internal const string childIndent = "│ ";
    internal const string childNodeIndent = "├─";
    internal const string leafNodeIndent = "└─";

    TextWriter _writer;

    internal NodeWriter(TextWriter writer)
    {
        _writer = writer;
    }

    string _ComposeChildIndent(string indent, bool last) => indent + (last? emptyIndent : childIndent);

    string _ComposeIndent(bool last) => last? leafNodeIndent : childNodeIndent;

    string _ComposeIndent(int index, bool last) => $"{_ComposeIndent(last)}[{index}]";

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
        {
            _writer.Write(indent);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            _writer.Write(_ComposeIndent(last));
            _writer.WriteLine($"{property.Name}: {_ComposeValue(value)}");
            Console.ResetColor();

        }
    }

    void _WriteEnumberable(IEnumerable<Node> nodes, string indent, bool last, string? name = null)
    {
        var length = nodes.Count();

        _writer.Write(indent);
        _writer.Write(_ComposeIndent(last));
        Console.ForegroundColor = ConsoleColor.Green;
        if (name is null)
            _writer.Write(nodes.GetType().Name);
        else
            _writer.Write(name);
        _writer.WriteLine($"[{length}]");
        Console.ResetColor();

        if (length == 0)
            return;

        indent = _ComposeChildIndent(indent, last);

        var index = 0;
        foreach(var node in nodes)
            _Write(node, indent, index >= length - 1, index++);
    }

    void _Write(Node node, string indent, bool last, int? index = null) 
    {
        _writer.Write(indent);
        _writer.Write(_ComposeIndent(last));
        if (index is not null)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            _writer.Write($"[{index}]");
        }
        else
            Console.ForegroundColor = ConsoleColor.Gray;
        _writer.WriteLine(node.GetType().Name);
        Console.ResetColor();

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