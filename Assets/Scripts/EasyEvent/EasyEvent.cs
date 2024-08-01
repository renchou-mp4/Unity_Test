using System;

public struct EasyEvent : IEquatable<EasyEvent>
{
    private EventDelegate _delegate;

    public EasyEvent(EventDelegate eventDelegate)
    {
        _delegate = eventDelegate;
    }

    public bool _Valid => _delegate != null;

    public EventHandle AddListener(Action callback)
    {
        this.EnsureValid();
        return _delegate.AddListener(callback);
    }

    public bool Remove(EventHandle handle)
    {
        this.EnsureValid();
        return _delegate.Remove(handle);
    }

    public int RemoveListener(Action callback)
    {
        this.EnsureValid();
        return _delegate.RemoveListener(callback);
    }

    public bool Equals(EasyEvent other)
    {
        this.EnsureValid();
        return Equals(_delegate, other._delegate);
    }

    public override int GetHashCode()
    {
        return _delegate == null ? 0 : _delegate.GetHashCode();
    }

    private void EnsureValid()
    {
        if (!_Valid)
        {
            throw new InvalidOperationException("EasyEvent ÊµÀý·Ç·¨£¡");
        }
    }
}
