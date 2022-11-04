namespace Utils;

public class TextLocation
{
    public int pos { get; }
    public int len { get; }

    public TextLocation(int pos, int len) 
    {
        this.pos = pos;
        this.len = len;
    }

    public static TextLocation Embrace(TextLocation a, TextLocation b)
    {
        return new TextLocation(a.pos, b.pos - a.pos + b.len);
    }
}
