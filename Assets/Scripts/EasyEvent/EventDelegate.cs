using System;
using System.Collections.Generic;

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

        if (handle._callback == null)
        {
            return false;
        }

        handle._callback = null;

        if (_lockCount == 0)
        {
            if (handle._next == handle)
            {
                //只有一个handle
                handle._next = handle._front = null;
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
        }

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
            if (tmpHandle._callback == action)
            {
                tmpHandle._callback = null;
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

    public void RemoveAllListeners()
    {
        EventHandle tmpHandle = _head;

        if (tmpHandle != null)
        {
            while (true)
            {
                tmpHandle._callback = null;
                tmpHandle = tmpHandle._next;
                if (tmpHandle == null || tmpHandle == _head)
                {
                    break;
                }
            }
        }
        _ListenerCount = 0;
    }

    public void Invoke()
    {
        InternalInvoke(false);
    }

    public void InvokeAll()
    {
        InternalInvoke(true);
    }

    private void InternalInvoke(bool continueWhenException)
    {
        if (_head == null)
        {
            return;
        }

        List<Exception> exceptions = null;

        _lockCount++;
        EventHandle tmpHandle = _head;
        while (true)
        {
            if (tmpHandle._callback != null)
            {
                try
                {
                    tmpHandle._callback();
                }
                catch (Exception e)
                {
                    if (!continueWhenException)
                    {
                        _lockCount--;
                        throw e;
                    }

                    if (exceptions == null)
                    {
                        exceptions = new List<Exception>();
                    }
                    exceptions.Add(e);
                }
                finally
                {
                    tmpHandle = tmpHandle._next;
                }

                if (tmpHandle == null || tmpHandle == _head)
                {
                    break;
                }
            }
            else
            {
                if (_lockCount == 1)
                {
                    if (tmpHandle._next == _head)
                    {
                        //只有一个handle
                        tmpHandle._next = tmpHandle._front = null;
                        _head = null;
                        break;
                    }
                    else if (tmpHandle == _head)
                    {
                        //handle就是head
                        tmpHandle._next._front = tmpHandle._front;
                        tmpHandle._front._next = tmpHandle._next;
                        _head = tmpHandle._next;
                        tmpHandle._next = null;
                        tmpHandle._front = null;
                    }
                    else
                    {
                        //其他情况
                        tmpHandle._next._front = tmpHandle._front;
                        tmpHandle._front._next = tmpHandle._next;
                        EventHandle nextHandle = tmpHandle._next;
                        tmpHandle._next = null;
                        tmpHandle._front = null;
                        tmpHandle = nextHandle;
                        if (tmpHandle == null || tmpHandle == _head)
                        {
                            break;
                        }
                    }
                }
            }
        }

        _lockCount--;
        if (exceptions != null)
        {
            throw new AggregateException(exceptions);
        }
    }
}
