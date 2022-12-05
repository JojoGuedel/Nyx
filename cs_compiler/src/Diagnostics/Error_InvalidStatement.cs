namespace Nyx.Diagnostics;

using CodeAnalysis;

public class Error_InvalidStatement : ADiagnostic
{
    private SyntaxNode _token;

    public Error_InvalidStatement(SyntaxNode token) : base(token.location)
    {
        _severity = DiagnosticSeverity.Error;
        _kind = DiagnosticKind.Error_InvalidStatement;

        _token = token;
    }

    public override string GetMessage()
    {
        return $"invalid statement <{_token.kind}>";
    }
}

