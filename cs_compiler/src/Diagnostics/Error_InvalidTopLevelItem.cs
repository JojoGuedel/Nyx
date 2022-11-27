using CodeAnalysis;

namespace Diagnostics;

public class Error_InvalidTopLevelItem : ADiagnostic
{
    private SyntaxNode _token;

    public Error_InvalidTopLevelItem(SyntaxNode token) : base(token.location)
    {
        _severity = DiagnosticSeverity.Error;
        _kind = DiagnosticKind.Error_InvalidStatement;

        _token = token;
    }

    public override string GetMessage()
    {
        return $"invalid topLevelItem <{_token.kind}>";
    }
}

