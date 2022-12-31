using System.Text;

namespace Nyx.Utils;

public class TextInfo
{
    public int length { get; }
    public List<LineInfo> lines { get; }

    public LineInfo this[int i]
    {
        get 
        {
            if (i < 0 || i >= lines.Count)
                return new LineInfo("", i, 0);
            return lines[i];
        }
    }

    public TextInfo(string text)
    {
        length = text.Length;
        lines = new List<LineInfo>();

        var line = string.Empty;
        var start = 0;

        for(int i = 0; i < text.Length; i++)
        {
            line += text[i];

            if (text[i] == '\n')
            {
                lines.Add(new LineInfo(line, lines.Count + 1, start));
                line = string.Empty;
                start = i;
            }
        }

        lines.Add(new LineInfo(line, lines.Count + 1, start));
    }

    public LineInfo GetLineAtIndex(int pos)
    {
        var last = 0;

        for(int i = 0; i < lines.Count; i++)
        {
            if (lines[i].start < pos)
                last = i;
            else
                break;
        }

        return lines[last];
    }

    public override string ToString()
    {
        if (lines.Count == 0)
            return "";

        StringBuilder ret = new StringBuilder();

        var maxLen = lines.Last().lineNumber.ToString().Length;
        foreach(var line in lines)
        {
            for(int i = 0; i < maxLen - line.lineNumber.ToString().Length; i++)
                ret.Append(" ");
            ret.Append(line.ToString());
        }    

        return ret.ToString();
    }
}
