class LexerMatchGroup
{
    public string Text { get; private set; }
    public List<LexerMatchGroup> Children { get; private set; }
    public SyntaxKind Kind { get; private set; }

    public LexerMatchGroup(): this("", SyntaxKind.NoMatch) {}
    public LexerMatchGroup(string text, SyntaxKind kind) {
        Text = text;
        Kind = kind;
        Children = new List<LexerMatchGroup>();
    }

    public void Split(int pos)
    {
        if (pos < 0 || pos >= Text.Length)
            return;

        var text = Text.Substring(pos);
        Text = Text.Substring(0, pos);

        var child = new LexerMatchGroup(text, Kind);
        child.Children = new List<LexerMatchGroup>(Children);
        Children.Clear();
        Children.Add(child);

        Kind = SyntaxKind.NoMatch;
    }

    public int GetMatchingLength(string text, int pos=0) 
    {
        int searchPos = 0;

        while ((searchPos < Text.Length) && (pos < text.Length) && (text[pos] == Text[searchPos]))
        {
            pos++;
            searchPos ++;
        }

        return searchPos;
    }

    public void Add(LexerMatchGroup lexMatchGroup) 
    {
        foreach(var child in Children)
        {
            var matchingLength = child.GetMatchingLength(lexMatchGroup.Text);

            if (matchingLength == 0)
                continue;
            
            else if (lexMatchGroup.Text == child.Text)
            {
                Kind = lexMatchGroup.Kind;
                return;
            }

            else if (matchingLength == lexMatchGroup.Text.Length)
            {
                child.Split(matchingLength);

                child.Kind = lexMatchGroup.Kind;
                child.Children.AddRange(lexMatchGroup.Children);

                return;
            }
            else
            {
                child.Split(matchingLength);

                lexMatchGroup.Text = lexMatchGroup.Text.Substring(matchingLength);
                child.Add(lexMatchGroup);
            }
        }

        Children.Add(lexMatchGroup);
    }

    public SyntaxKind Match(string text, int pos=0) 
    {
        foreach(var child in Children)
        {
            var matchingLength = child.GetMatchingLength(text, pos);

            if (matchingLength == 0)
                continue;

            else if (matchingLength == text.Length - pos)
                return child.Kind;
            
            else
                return child.Match(text, pos + matchingLength);
        }

        return SyntaxKind.InvalidMatch;
    }
}
