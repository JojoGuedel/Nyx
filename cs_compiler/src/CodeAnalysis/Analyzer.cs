namespace Nyx.CodeAnalysis;

public abstract class Analyzer<InputT, OutputT>
{
    protected readonly List<InputT> _values;
    protected readonly InputT _terminator;

    public bool isFinished { get; private set; }
    protected int _pos;

    protected Analyzer(List<InputT> values, InputT terminator)
    {
        _values = values;
        _terminator = terminator;

        isFinished = false;
        _pos = 0;
    }

    protected void _IncrementPos()
    {
        if (!isFinished)
            _pos++;

        else if (_pos >= _values.Count)
            isFinished = true;
    }

    protected InputT _ReadNext()
    {
        var val = _Peek(0);
        _IncrementPos();
        return val;
    }

    protected InputT _Peek(int offset = 0)
    {
        if (_pos + offset < 0 || _pos + offset >= _values.Count)
            return _terminator;

        return _values[_pos + offset];
    }

    public abstract IEnumerable<OutputT> GetAll();
}