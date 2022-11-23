namespace CodeAnalysis;

abstract class AAnalyzer<InputT, OutputT>
{
    protected readonly List<InputT> _values;
    protected readonly InputT _terminator;

    protected int _pos;

    protected AAnalyzer(List<InputT> values, InputT terminator)
    {
        _values = values;
        _terminator = terminator;

        _pos = 0;
    }

    protected void _IncrementPos()
    {
        if (_pos <= _values.Count) _pos++;
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