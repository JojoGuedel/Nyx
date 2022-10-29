namespace SyntaxAnalysis;

class SyntaxDefinition
{
    private Dictionary<char, SyntaxTokenKind> _singleTokens;
    private Dictionary<(char, char), SyntaxTokenKind> _doubleTokens;

    public SyntaxDefinition()
    {
        _singleTokens = new Dictionary<char, SyntaxTokenKind>();
        _doubleTokens = new Dictionary<(char, char), SyntaxTokenKind>();
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