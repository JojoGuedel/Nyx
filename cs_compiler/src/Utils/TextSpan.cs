namespace Utils;

class TextSpan
{
    public string Text { get; }
    public TextLocation Location { get; }

    public TextSpan(string text, int pos, int len): this(text, new TextLocation(pos, len)) {}
    public TextSpan(string text, TextLocation location) 
    {
        Text = text;
        Location = location;
    }
}
