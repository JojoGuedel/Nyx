using Utils;

namespace SyntaxAnalysis;

class Lexer
{
    public string Text { get; }
    public SyntaxDefinition Syntax { get; }
    
    private int pos;

    public Lexer(string text, SyntaxDefinition syntax)
    {
        Text = text;
        Syntax = syntax;
    }

    private char Peek(int offset = 0)
    {
        if (pos + offset < 0 || pos + offset >= Text.Length)
            return '\0';

        return Text[pos + offset];
    }

    private string SubText(int start, int len)
    {
        var current = String.Empty;

        for (int i = 0; i < len; i++)
            current += Peek(start - pos + i);

        return current;
    }

    public SyntaxToken NextToken()
    {
        var kind = SyntaxTokenKind.InvalidChar;
        var start = pos;

        if (char.IsDigit(Peek()))
            kind = LexDigit();
        else
        {
            (var match, kind) = Syntax.Search(SubText(pos, Text.Length - pos + 1));
            pos += match.Length;
        }

        var len = pos - start;
        var text = SubText(start, len);
        var span = new TextSpan(text, pos, len);

        return new SyntaxToken(kind, span);
    }

    public SyntaxTokenKind LexDigit()
    {
        do pos++;
        while (char.IsDigit(Peek()));

        return SyntaxTokenKind.Int;
    }
}