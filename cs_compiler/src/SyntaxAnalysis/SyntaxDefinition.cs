namespace SyntaxAnalysis;

public class SyntaxDefinition
{
    public int indentSize { get; }
    public char indentSymbol { get; }
    public char newLineSymbol { get; }
    public char endSymbol { get; }

    private Dictionary<char, SyntaxKind> _singleTokens;
    private Dictionary<(char, char), SyntaxKind> _doubleTokens;

    private Dictionary<string, SyntaxKind> _keywords;

    public SyntaxDefinition(
        int indentSize=4,
        char indentSymbol=' ',
        char newLineSymbol='\n',
        char endSymbol='\u0000'
    )
    {
        _singleTokens = new Dictionary<char, SyntaxKind>();
        _doubleTokens = new Dictionary<(char, char), SyntaxKind>();

        _keywords = new Dictionary<string, SyntaxKind>();

        this.indentSize = indentSize;
        this.indentSymbol = indentSymbol;

        this.newLineSymbol = newLineSymbol;
        DefineSingleToken(newLineSymbol, SyntaxKind.Token_NewLine);

        this.endSymbol = endSymbol;
        DefineSingleToken(endSymbol, SyntaxKind.Token_End);
    }

    public void DefineSingleToken(char pattern, SyntaxKind kind)
    {
        _singleTokens.Add(pattern, kind);
    }

    public void DefineDoubleToken((char, char) pattern, SyntaxKind kind)
    {
        _doubleTokens.Add(pattern, kind);
    }

    public SyntaxKind GetSingleTokenKind(char pattern)
    {
        var kind = SyntaxKind._Error;
        _singleTokens.TryGetValue(pattern, out kind);
        return kind;
    }

    public SyntaxKind GetDoubleTokenKind((char, char) pattern)
    {
        var kind = SyntaxKind._Error;
        _doubleTokens.TryGetValue(pattern, out kind);
        return kind;
    }

    public void DefineKeyword(string pattern, SyntaxKind kind)
    {
        _keywords.Add(pattern, kind);
    }

    public SyntaxKind GetKeyword(string pattern)
    {
        var kind = SyntaxKind._Error;
        _keywords.TryGetValue(pattern, out kind);
        return kind;
    }

    public bool IsLineTerminator(char pattern)
    {
        return
            pattern == newLineSymbol ||
            pattern == endSymbol;
    }

    public bool IsWhiteSpace(SyntaxKind kind)
    {
        return
            kind == SyntaxKind.Token_Indent ||
            kind == SyntaxKind.Token_NewLine;
    }
}