namespace Nyx.Utils;

public class TextLocation
{
    public int pos { get; }
    public int len { get; }
    public int end { get => pos + len; }

    public TextLocation(int pos, int len) 
    {
        this.pos = pos;
        this.len = len;
    }

    public static TextLocation Embrace(TextLocation a, TextLocation b)
    {
        return new TextLocation(a.pos, b.end - a.pos);
    }
}
