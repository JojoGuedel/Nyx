namespace SyntaxAnalysis;

public enum SyntaxKind
{
    // Token_Error,
    _Error,
    _Discard,
    Token_End,
    Token_InvalidChar,

    _Indent,
    Token_NewBlock,
    Token_EndBlock,
    Token_NewLine,
    Token_Number,
    _StringMarker,
    Token_String,
    Token_Identifier,
    Token_UnusedIdentifier,

    Token_LParen,
    Token_RParen,
    Token_LSquare,
    Token_RSquare,

    Token_Dot,
    Token_Comma,
    Token_Colon,
    Token_Semicolon,

    Token_Plus,
    Token_Minus,
    Token_Star,
    Token_Slash,

    Token_Less,
    Token_Greater,
    Token_Equal,
    Token_Percent,

    // Token_LBrace,
    // Token_RBrace,
    Token_EqualEqual,
    Token_NotEqual,
    Token_LessEqual,
    Token_GreaterEqual,

    Token_PlusPlus,
    Token_PlusEqual,
    Token_MinusMinus,
    Token_MinusEqual,
    Token_StarEqual,
    Token_SlashEqual,
    Token_PercentEqual,

    Token_RArrow,

    Keyword_Funcion,
    Keyword_Let,
    Keyword_If,
    Keyword_Else,
    Keyword_For,
    Keyword_Do,
    Keyword_While,

    Syntax_Empty,

    Syntax_DeclarationStatement,
    Syntax_TypeClause,

    Syntax_Addition,
    Syntax_Subtraction,
    Syntax_Multiplication,
    Syntax_Division,
    Syntax_Error,
    Syntax_CompilationUnit,
}
