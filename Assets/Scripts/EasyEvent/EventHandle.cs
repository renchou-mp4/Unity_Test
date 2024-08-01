using System;

public class EventHandle
{
    internal EventHandle _front;
    internal EventHandle _next;
    internal Action _callback;
    internal EventDelegate _ower;

    internal EventHandle(Action callback, EventDelegate ower)
    {
        _callback = callback;
        _ower = ower;
    }
}
