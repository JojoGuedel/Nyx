namespace Nyx.Diagnostics;

using Analysis;

public class InvalidTopLevelItem : Diagnostic
{
    private SyntaxNode _token;

    public InvalidTopLevelItem(SyntaxNode token) : 
        base(DiagnosticSeverity.Error, DiagnosticKind.Error_InvalidStatement, DiagnosticOrigin.LexicalAnalyisis, token.location)
    {
        _token = token;
    }

    public override string GetMessage()
    {
        return $"invalid topLevelItem <{_token.kind}>";
    }
}

