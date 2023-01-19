using Nyx.Diagnostics;

namespace Nyx.Analysis;

public abstract class Analyzer<InputT, OutputT>
{
    protected readonly List<InputT> _values;
    protected readonly InputT _terminator;

    public DiagnosticCollection diagnostics { get; }

    protected int _pos;
    protected InputT _last { get => _Peek(-1); }
    protected InputT _current { get => _Peek(0); }
    protected InputT _next { get => _Peek(1); }
    public bool isFinished { get => _pos >= _values.Count; }

    protected Analyzer(List<InputT> values, InputT terminator)
    {
        _values = values;
        _terminator = terminator;

        diagnostics = new DiagnosticCollection();

        _pos = 0;
    }

    protected void _IncrementPos()
    {
        if (!isFinished)
            _pos++;
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