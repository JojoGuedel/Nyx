namespace Nyx.Analysis;

public class _SyntaxInfo
{
    public int indentSize { get; }
    public char escapeSymbol { get; }
    public char newLineSymbol { get; }
    public char endSymbol { get; }

    public int maxOperatorPrecedence { get; }

    public Dictionary<char, SyntaxKind> singleTokens { get; }
    public Dictionary<(char, char), SyntaxKind> doubleTokens { get; }

    public Dictionary<string, SyntaxKind> keywords { get; }

    public _SyntaxInfo(
        int indentSize=4,
        char escapeSymbol = '\\',
        char lineEndSymbol = '\n',
        char endSymbol='\u0000'
    )
    {
        singleTokens = new Dictionary<char, SyntaxKind>();
        doubleTokens = new Dictionary<(char, char), SyntaxKind>();

        keywords = new Dictionary<string, SyntaxKind>();

        this.indentSize = indentSize;

        this.escapeSymbol = escapeSymbol;

        this.newLineSymbol = lineEndSymbol;
        DefineSingleToken(lineEndSymbol, SyntaxKind.Token_NewLine);

        this.endSymbol = endSymbol;
        DefineSingleToken(endSymbol, SyntaxKind.Token_End);

        maxOperatorPrecedence = 7;
    }

    public void DefineSingleToken(char pattern, SyntaxKind kind)
    {
        singleTokens.Add(pattern, kind);
    }

    public SyntaxKind GetSingleTokenKind(char pattern)
    {
        var kind = SyntaxKind.Token_Error;
        singleTokens.TryGetValue(pattern, out kind);
        return kind;
    }

    public void DefineDoubleToken((char, char) pattern, SyntaxKind kind)
    {
        doubleTokens.Add(pattern, kind);
    }

    public SyntaxKind GetDoubleTokenKind((char, char) pattern)
    {
        var kind = SyntaxKind.Token_Error;
        doubleTokens.TryGetValue(pattern, out kind);
        return kind;
    }

    public void DefineKeyword(string pattern, SyntaxKind kind)
    {
        keywords.Add(pattern, kind);
    }

    public SyntaxKind GetKeyword(string pattern)
    {
        var kind = SyntaxKind.Token_Error;
        keywords.TryGetValue(pattern, out kind);
        return kind;
    }

    public bool IsLineTerminator(char pattern)
    {
        return
            pattern == newLineSymbol ||
            pattern == endSymbol;
    }

    public bool IsLineTerminator(SyntaxKind kind)
    {
        return
            kind == SyntaxKind.Token_NewLine ||
            kind == SyntaxKind.Token_End;
    }

    public bool IsWhiteSpace(SyntaxKind kind)
    {
        return
            kind == SyntaxKind.Token_Indent ||
            kind == SyntaxKind.Token_Space ||
            kind == SyntaxKind.Token_NewLine;
    }

    public bool IsBlockTerminator(SyntaxKind kind)
    {
        return 
            kind == SyntaxKind.Token_EndBlock ||
            kind == SyntaxKind.Token_End;
    }

    public int BinaryOperatorPrecedence(SyntaxKind kind)
    {
        switch (kind)
        {
            case SyntaxKind.Token_Equal:
            case SyntaxKind.Token_PlusEqual:
            case SyntaxKind.Token_MinusEqual:
            case SyntaxKind.Token_StarEqual:
            case SyntaxKind.Token_SlashEqual:
            case SyntaxKind.Token_PercentEqual:
                return 7;
            case SyntaxKind.Keyword_Or:
                return 6;
            case SyntaxKind.Keyword_And:
                return 5;
            case SyntaxKind.Token_EqualEqual:
            case SyntaxKind.Token_NotEqual:
                return 4;
            case SyntaxKind.Token_Greater:
            case SyntaxKind.Token_GreaterEqual:
            case SyntaxKind.Token_Less:
            case SyntaxKind.Token_LessEqual:
                return 3;
            case SyntaxKind.Token_Plus:
            case SyntaxKind.Token_Minus:
                return 2;
            case SyntaxKind.Token_Star:
            case SyntaxKind.Token_Slash:
                return 1;
            default: 
                return 0;
        }
    }

    public static _SyntaxInfo Default()
    {
        var syntax = new _SyntaxInfo();

        syntax.DefineSingleToken('\r', SyntaxKind.Token_Discard);
        syntax.DefineSingleToken(' ', SyntaxKind.Token_Space);
        syntax.DefineDoubleToken(('/', '/'), SyntaxKind.Token_CommentMarker);
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

        syntax.DefineKeyword("struct", SyntaxKind.Keyword_Struct);
        syntax.DefineKeyword("global", SyntaxKind.Keyword_Global);
        syntax.DefineKeyword("static", SyntaxKind.Keyword_Static);
        syntax.DefineKeyword("mut", SyntaxKind.Keyword_Mutable);
        syntax.DefineKeyword("var", SyntaxKind.Keyword_Var);
        syntax.DefineKeyword("func", SyntaxKind.Keyword_Function);
        syntax.DefineKeyword("return", SyntaxKind.Keyword_Return);
        syntax.DefineKeyword("if", SyntaxKind.Keyword_If);
        syntax.DefineKeyword("else", SyntaxKind.Keyword_Else);
        syntax.DefineKeyword("and", SyntaxKind.Keyword_And);
        syntax.DefineKeyword("or", SyntaxKind.Keyword_Or);
        // syntax.DefineKeyword("switch", SyntaxKind.Keyword_Switch);
        // syntax.DefineKeyword("case", SyntaxKind.Keyword_Case);
        // syntax.DefineKeyword("default", SyntaxKind.Keyword_Default);
        // syntax.DefineKeyword("for", SyntaxKind.Keyword_For);
        // syntax.DefineKeyword("in", SyntaxKind.Keyword_In);
        // syntax.DefineKeyword("do", SyntaxKind.Keyword_Do);
        syntax.DefineKeyword("while", SyntaxKind.Keyword_While);
        syntax.DefineKeyword("continue", SyntaxKind.Keyword_While);
        syntax.DefineKeyword("break", SyntaxKind.Keyword_While);

        return syntax;
    }
}