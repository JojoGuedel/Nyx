namespace CodeAnalysis;

public class SyntaxDefinition
{
    public int indentSize { get; }
    public char lineEndSymbol { get; }
    public char endSymbol { get; }

    private Dictionary<char, SyntaxKind> _singleTokens;
    private Dictionary<(char, char), SyntaxKind> _doubleTokens;

    private Dictionary<string, SyntaxKind> _keywords;

    public SyntaxDefinition(
        int indentSize=4,
        char lineEndSymbol = '\n',
        char endSymbol='\u0000'
    )
    {
        _singleTokens = new Dictionary<char, SyntaxKind>();
        _doubleTokens = new Dictionary<(char, char), SyntaxKind>();

        _keywords = new Dictionary<string, SyntaxKind>();

        this.indentSize = indentSize;

        this.lineEndSymbol = lineEndSymbol;
        DefineSingleToken(lineEndSymbol, SyntaxKind.Token_NewLine);

        this.endSymbol = endSymbol;
        DefineSingleToken(endSymbol, SyntaxKind.Token_End);
    }

    public void DefineSingleToken(char pattern, SyntaxKind kind)
    {
        _singleTokens.Add(pattern, kind);
    }

    public SyntaxKind GetSingleTokenKind(char pattern)
    {
        var kind = SyntaxKind.Token_Error;
        _singleTokens.TryGetValue(pattern, out kind);
        return kind;
    }

    public void DefineDoubleToken((char, char) pattern, SyntaxKind kind)
    {
        _doubleTokens.Add(pattern, kind);
    }

    public SyntaxKind GetDoubleTokenKind((char, char) pattern)
    {
        var kind = SyntaxKind.Token_Error;
        _doubleTokens.TryGetValue(pattern, out kind);
        return kind;
    }

    public void DefineKeyword(string pattern, SyntaxKind kind)
    {
        _keywords.Add(pattern, kind);
    }

    public SyntaxKind GetKeyword(string pattern)
    {
        var kind = SyntaxKind.Token_Error;
        _keywords.TryGetValue(pattern, out kind);
        return kind;
    }

    public bool IsLineTerminator(char pattern)
    {
        return
            pattern == lineEndSymbol ||
            pattern == endSymbol;
    }

    public bool IsWhiteSpace(SyntaxKind kind)
    {
        return
            kind == SyntaxKind.Token_Indent ||
            kind == SyntaxKind.Token_Space ||
            kind == SyntaxKind.Token_NewLine;
    }

    public static SyntaxDefinition Default()
    {
        var syntax = new SyntaxDefinition();

        syntax.DefineSingleToken('\r', SyntaxKind.Token_Discard);
        syntax.DefineSingleToken(' ', SyntaxKind.Token_Space);
        syntax.DefineDoubleToken(('/', '/'), SyntaxKind.Token_CommentMarker);

        syntax.DefineSingleToken('\n', SyntaxKind.Token_NewLine);
        syntax.DefineSingleToken('"', SyntaxKind.Token_StringMarker);

        syntax.DefineSingleToken('(', SyntaxKind.Token_LParen);
        syntax.DefineSingleToken(')', SyntaxKind.Token_RParen);
        syntax.DefineSingleToken('[', SyntaxKind.Token_LSquare);
        syntax.DefineSingleToken(']', SyntaxKind.Token_RSquare);

        syntax.DefineSingleToken('.', SyntaxKind.Token_Dot);
        syntax.DefineSingleToken(',', SyntaxKind.Token_Comma);
        syntax.DefineSingleToken(':', SyntaxKind.Token_Colon);
        syntax.DefineSingleToken(';', SyntaxKind.Token_Semicolon);
        syntax.DefineDoubleToken(('-', '>'), SyntaxKind.Token_RArrow);

        syntax.DefineSingleToken('+', SyntaxKind.Token_Plus);
        syntax.DefineSingleToken('-', SyntaxKind.Token_Minus);
        syntax.DefineSingleToken('*', SyntaxKind.Token_Star);
        syntax.DefineSingleToken('/', SyntaxKind.Token_Slash);

        syntax.DefineSingleToken('<', SyntaxKind.Token_Less);
        syntax.DefineSingleToken('>', SyntaxKind.Token_Greater);
        syntax.DefineSingleToken('=', SyntaxKind.Token_Equal);
        syntax.DefineSingleToken('%', SyntaxKind.Token_Percent);

        // syntax.DefineSingleToken('{', SyntaxKind.Token_LBrace);
        // syntax.DefineSingleToken('}', SyntaxKind.Token_RBrace);
        syntax.DefineDoubleToken(('=', '='), SyntaxKind.Token_EqualEqual);
        syntax.DefineDoubleToken(('!', '='), SyntaxKind.Token_NotEqual);
        syntax.DefineDoubleToken(('<', '='), SyntaxKind.Token_LessEqual);
        syntax.DefineDoubleToken(('>', '='), SyntaxKind.Token_GreaterEqual);

        syntax.DefineDoubleToken(('+', '+'), SyntaxKind.Token_PlusPlus);
        syntax.DefineDoubleToken(('+', '='), SyntaxKind.Token_PlusEqual);
        syntax.DefineDoubleToken(('-', '-'), SyntaxKind.Token_MinusMinus);
        syntax.DefineDoubleToken(('-', '='), SyntaxKind.Token_MinusEqual);
        syntax.DefineDoubleToken(('*', '='), SyntaxKind.Token_StarEqual);
        syntax.DefineDoubleToken(('/', '='), SyntaxKind.Token_SlashEqual);
        syntax.DefineDoubleToken(('%', '='), SyntaxKind.Token_PercentEqual);

        syntax.DefineKeyword("public", SyntaxKind.Keyword_Public);
        syntax.DefineKeyword("static", SyntaxKind.Keyword_Static);
        syntax.DefineKeyword("abstract", SyntaxKind.Keyword_Abstract);
        syntax.DefineKeyword("section", SyntaxKind.Keyword_Section);
        syntax.DefineKeyword("struct", SyntaxKind.Keyword_Struct);
        syntax.DefineKeyword("include", SyntaxKind.Keyword_Include);
        syntax.DefineKeyword("func", SyntaxKind.Keyword_Function);
        syntax.DefineKeyword("var", SyntaxKind.Keyword_Var);
        syntax.DefineKeyword("mut", SyntaxKind.Keyword_Mut);
        syntax.DefineKeyword("if", SyntaxKind.Keyword_If);
        syntax.DefineKeyword("else", SyntaxKind.Keyword_Else);
        syntax.DefineKeyword("switch", SyntaxKind.Keyword_Switch);
        syntax.DefineKeyword("for", SyntaxKind.Keyword_For);
        syntax.DefineKeyword("do", SyntaxKind.Keyword_Do);
        syntax.DefineKeyword("while", SyntaxKind.Keyword_While);

        return syntax;
    }
}