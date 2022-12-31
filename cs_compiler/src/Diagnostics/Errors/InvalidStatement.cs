namespace Nyx.Diagnostics;

using Analysis;

public class InvalidStatement : Diagnostic
{
    private SyntaxNode _token;

    public InvalidStatement(SyntaxNode token) :
        base(DiagnosticSeverity.Error, DiagnosticKind.Error_InvalidStatement, DiagnosticOrigin.LexicalAnalyisis, token.location)
    {
        _token = token;
    }

    public override string GetMessage()
    {
        return $"invalid statement <{_token.kind}>";
    }
}

