namespace SyntaxAnalysis;

public class SyntaxDefinition
{
    public int indentSize { get; }
    public char indentSymbol { get; }
    public char newLineSymbol { get; }
    public char endSymbol { get; }

    private Dictionary<char, SyntaxTokenKind> _singleTokens;
    private Dictionary<(char, char), SyntaxTokenKind> _doubleTokens;

    public SyntaxDefinition(
        int indentSize=4,
        char indentSymbol=' ',
        char newLineSymbol='\n',
        char endSymbol='\u0000'
    )
    {
        _singleTokens = new Dictionary<char, SyntaxTokenKind>();
        _doubleTokens = new Dictionary<(char, char), SyntaxTokenKind>();

        this.indentSize = indentSize;
        this.indentSymbol = indentSymbol;

        this.newLineSymbol = newLineSymbol;
        DefineSingleToken(newLineSymbol, SyntaxTokenKind.NEWLINE);

        this.endSymbol = endSymbol;
        DefineSingleToken(endSymbol, SyntaxTokenKind.END);
    }

    public void DefineSingleToken(char pattern, SyntaxTokenKind kind)
    {
        _singleTokens.Add(pattern, kind);
    }

    public void DefineDoubleToken((char, char) pattern, SyntaxTokenKind kind)
    {
        _doubleTokens.Add(pattern, kind);
    }

    public SyntaxTokenKind GetSingleTokenKind(char pattern)
    {
        var kind = SyntaxTokenKind.ERROR;
        _singleTokens.TryGetValue(pattern, out kind);
        return kind;
    }

    public SyntaxTokenKind GetDoubleTokenKind((char, char) pattern)
    {
        var kind = SyntaxTokenKind.ERROR;
        _doubleTokens.TryGetValue(pattern, out kind);
        return kind;
    }
}