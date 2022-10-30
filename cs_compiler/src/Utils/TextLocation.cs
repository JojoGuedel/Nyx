namespace Utils;

public class TextLocation
{
    public int Pos { get; }
    public int Len { get; }

    public TextLocation(int pos, int len) 
    {
        Pos = pos;
        Len = len;
    }
}
