namespace Nyx.Diagnostics;

using Analysis;

public class InvalidToplevelItem : Diagnostic
{
    private SyntaxNode _token;

    public InvalidToplevelItem(SyntaxNode token) : 
        base(DiagnosticSeverity.Error, DiagnosticKind.Error_InvalidStatement, DiagnosticOrigin.LexicalAnalysis, token.location)
    {
        _token = token;
    }

    public override string GetMessage()
    {
        return $"Unexpected token kind <{_token.kind}> as top level item.";
    }
}

