class SyntaxDefinition
{
    private Dictionary<string, SyntaxKind> syntaxDefs;

    public SyntaxDefinition()
    {
        syntaxDefs = new Dictionary<string, SyntaxKind>();
    }

    public void Define(string pattern, SyntaxKind kind)
    {
        syntaxDefs.Add(pattern, kind);
    }

    public SyntaxKind Match(string pattern)
    {
        try
        {
            return syntaxDefs[pattern];
        }
        catch
        {
            return SyntaxKind.UndefinedPattern;
        }
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

    public (string, SyntaxKind) Search(string text) 
    {
        var lastMatch = text[0].ToString();
        List<string>? possibleMatches = null;

        for(var pos = 0; pos < text.Length; pos++)
        {
            var pattern = text.Substring(0, pos + 1);
            possibleMatches = GetPossibleMatches(pattern, possibleMatches);

            if (possibleMatches.Count == 0)
                break;

            lastMatch = possibleMatches[0];
        }

        return (lastMatch, Match(lastMatch));
    }
}
