namespace SyntaxAnalysis;

class SyntaxDefinition
{
    private Dictionary<string, SyntaxTokenKind> syntaxDefs;

    public SyntaxDefinition()
    {
        syntaxDefs = new Dictionary<string, SyntaxTokenKind>();
    }

    public void Define(string pattern, SyntaxTokenKind kind)
    {
        syntaxDefs.Add(pattern, kind);
    }

    public SyntaxTokenKind Match(string pattern)
    {
        if (syntaxDefs.TryGetValue(pattern, out var kind))
            return kind;
        
        return SyntaxTokenKind.UndefinedPattern;
    }

    public List<string> GetPossibleMatches(string pattern, List<string>? patterns=null)
    {
        var possibleMatches = new List<string>();

        if (patterns is null)
            patterns = syntaxDefs.Keys.ToList<string>();
        
        foreach(var key in patterns)
        {
            if (key.StartsWith(pattern))
                possibleMatches.Add(key);
        }

        return possibleMatches;
    }

    public (string, SyntaxTokenKind) Search(string text) 
    {
        var lastMatchPattern = String.Empty;
        var lastMatchKind = SyntaxTokenKind.UndefinedPattern;
        List<string>? possibleMatches = null;

        for(var pos = 0; pos < text.Length; pos++)
        {
            var pattern = text.Substring(0, pos + 1);
            possibleMatches = GetPossibleMatches(pattern, possibleMatches);

            if (possibleMatches.Count == 0)
                break;

            lastMatchPattern = pattern;
            lastMatchKind = Match(pattern);
        }

        return (lastMatchPattern, lastMatchKind);
    }
}
