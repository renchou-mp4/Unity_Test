using System;

public class EventDelegate
{
    private int _lockCount;
    private EventHandle _head;
    public int _ListenerCount;

    public EventHandle AddListener(Action action)
    {
        if (action == null)
        {
            throw new ArgumentNullException("action");
        }

        EventHandle newHandle = new EventHandle(action, this);

        if (_head == null)
        {
            _head = newHandle;
            _head._front = _head;
            _head._next = _head;
        }
        else
        {
            newHandle._next = _head;
            newHandle._front = _head._front;
            _head._front._next = newHandle;
            _head._front = newHandle;
        }

        _ListenerCount++;

        return newHandle;
    }

    public bool Remove(EventHandle handle)
    {
        if (handle == null)
        {
            throw new ArgumentNullException(nameof(handle));
        }

        if (_head == null)
        {
            return false;
        }

        if (handle._ower != this)
        {
            return false;
        }

        if (handle._action == null)
        {
            return false;
        }

        if (handle._next == handle)
        {
            //只有一个handle
            _head = null;
        }
        else if (_head == handle)
        {
            //handle是head
            _head._next._front = _head._front;
            _head._front._next = _head._next;
            _head = handle._next;
        }
        else
        {
            //其他情况
            handle._front._next = handle._next;
            handle._next._front = handle._front;
        }

        handle._next = null;
        handle._front = null;

        _ListenerCount--;

        return true;
    }

    public int RemoveListener(Action action)
    {
        if (action == null)
        {
            throw new ArgumentNullException(nameof(action));
        }

        if (_head == null)
        {
            return 0;
        }

        int oldListenerCount = _ListenerCount;

        EventHandle tmpHandle = _head;
        while (true)
        {
            if (tmpHandle._action == action)
            {
                tmpHandle._action = null;
                _ListenerCount--;
            }

            tmpHandle = tmpHandle._next;

            if (tmpHandle == null || tmpHandle == _head)
            {
                break;
            }
        }

        return oldListenerCount - _ListenerCount;
    }

    public class EventHandle
    {
        internal EventHandle _front;
        internal EventHandle _next;
        internal Action _action;
        internal EventDelegate _ower;

        internal EventHandle(Action action, EventDelegate ower)
        {
            _action = action;
            _ower = ower;
        }
    }
}
