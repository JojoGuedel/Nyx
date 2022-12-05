using Xunit;
using Nyx.CodeAnalysis;

namespace Nyx.Tests;

public class LexicalAnalyzerTests
{
    static SyntaxDefinition _syntax = SyntaxDefinition.Default();

    [Theory]
    [MemberData(nameof(GetDoubleTokenData))]
    void TestTokens(string input, SyntaxKind expected)
    {
        var lexicalAnalyzer = new LexicalAnalyzer(_syntax, input);
        var result = lexicalAnalyzer.GetAll().ToList();

        Assert.Equal(2, result.Count);
        Assert.Equal(expected, result[0].kind);
        Assert.Equal(SyntaxKind.Token_End, result[1].kind);
    }

    static IEnumerable<object[]> GetDoubleTokenData()
    {
        object[] FinalizeTokenData(string pattern, SyntaxKind expected)
        {
            switch (expected)
            {
                case SyntaxKind.Token_CommentMarker:
                    pattern = "// test comment";
                    expected = SyntaxKind.Token_Comment;
                    break;
                case SyntaxKind.Token_StringMarker:
                    pattern = "\"test string\"";
                    expected = SyntaxKind.Token_String;
                    break;
            }

            return new object[] { pattern, expected };
        }

        // yield return new object[] { "//", SyntaxKind.Token_Comment };
        foreach (var pattern in _syntax.doubleTokens.Keys)
            yield return FinalizeTokenData("" + pattern.Item1 + pattern.Item2, _syntax.GetDoubleTokenKind(pattern));
    }
}