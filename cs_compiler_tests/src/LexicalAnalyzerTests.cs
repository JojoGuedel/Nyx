using Xunit;
using Nyx.CodeAnalysis;

namespace Nyx.Tests;

public class LexicalAnalyzerTests
{
    static SyntaxDefinition _syntax = SyntaxDefinition.Default();

    [Theory]
    [MemberData(nameof(GetSingleTokenData))]
    [MemberData(nameof(GetDoubleTokenData))]
    [MemberData(nameof(GetKeywordData))]
    [MemberData(nameof(GetAdditionalTokenData))]
    // TODO: make this work.
    void TestTokens(string input, params SyntaxKind[] expected)
    {
        var lexicalAnalyzer = new LexicalAnalyzer(_syntax, input);
        var result = lexicalAnalyzer.GetAll().ToList();

        for (int i = 0; i < expected.Length; i++)
            Assert.Equal(expected[i], result[i].kind);

        Assert.Equal(expected.Length + 1, result.Count);
        Assert.Equal(SyntaxKind.Token_End, result.Last().kind);
    }

    // ------------------------------ Basic Token Tests -----------------------------
    static object[]? FinalizeTokenData(string pattern, SyntaxKind expected)
    {

        switch (expected)
        {
            case SyntaxKind.Token_Space:
            case SyntaxKind.Token_End:
                return null;
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


    static IEnumerable<object[]> GetSingleTokenData()
    {
        foreach (var pattern in _syntax.singleTokens.Keys)
        {
            var data = FinalizeTokenData(pattern.ToString(), _syntax.GetSingleTokenKind(pattern));
            
            if (data is null) continue;
            yield return data;
        }
    }

    static IEnumerable<object[]> GetDoubleTokenData()
    {
        foreach (var pattern in _syntax.doubleTokens.Keys)
        {
            var data = FinalizeTokenData("" + pattern.Item1 + pattern.Item2, _syntax.GetDoubleTokenKind(pattern));
            
            if (data is null) continue;
            yield return data;
        }
    }

    static IEnumerable<object[]> GetKeywordData()
    {
        foreach (var pattern in _syntax.keywords.Keys)
        {
            var data = FinalizeTokenData(pattern, _syntax.GetKeyword(pattern));
            
            if (data is null) continue;
            yield return data;
        }
    }

    static IEnumerable<object[]> GetAdditionalTokenData()
    {
        string[] numberData = 
        {
            "1",
            "0000000001",
            "9487463973",
            // "0.9487463973",
            // "0x5aBCdeF",
            // "0b1010010",
            // "1e99",
            // "1.582092e99",
        };

        var expected = SyntaxKind.Token_Number;
        
        foreach(var e in numberData)
            yield return new object[] { e, expected };
    }
    // ------------------------------ Basic Token Tests -----------------------------

    // ---------------------------- Combined Token Tests ----------------------------

    // TODO
    
    // ---------------------------- Combined Token Tests ----------------------------
}