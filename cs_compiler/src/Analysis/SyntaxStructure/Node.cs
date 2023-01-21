using System.Collections.Immutable;
using Nyx.Utils;

namespace Nyx.Analysis;

public abstract class Node
{
    public TextLocation location { get; }

    public Node(TextLocation location)
    {
        this.location = location;
    }

    public abstract void Write(TextWriter writer, string indent, bool isLast);

    protected string _ChildIndent(bool isLast) => isLast? NodeWriterIndents.indent : NodeWriterIndents.childIndent;

    protected void _WriteName(TextWriter writer, string indent, bool isLast, string name)
    {
        writer.WriteLine($"{indent}{(isLast? NodeWriterIndents.leafNode : NodeWriterIndents.childNode)}{name}");
    }

    protected void _WriteString(TextWriter writer, string indent, bool isLast, string value)
    {
        _WriteName(writer, indent, isLast, $"C#String: \"{value}\"");
    }

    protected void _WriteKind(TextWriter writer, string indent, bool isLast, SyntaxKind kind)
    {
        _WriteName(writer, indent, isLast, $"Kind: {kind}");
    }
    
    protected void _WriteArray(TextWriter writer, string indent, bool isLast, string name, Node[] array)
    {
        _WriteName(writer, indent, isLast, $"{name}[]");
        indent += _ChildIndent(isLast);

        if (array.Length == 0)
            _WriteName(writer, indent, true, "Empty");

        isLast = false;
        for(int i = 0; i < array.Length; i++)
        {
            if (i == array.Length - 1)
                isLast = true;

            array[i].Write(writer, indent, isLast);
        }
    }
}
