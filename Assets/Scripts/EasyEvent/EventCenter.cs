using System;
using System.Collections.Generic;

public class EventCenter
{
    public static EventCenter _Defaule = new EventCenter();

    private Dictionary<string, EventDelegate> _mappings = new Dictionary<string, EventDelegate>();

    public EventHandle AddListener(string eventName, Action callback)
    {
        return this.GetEvent(eventName, true).AddListener(callback);
    }



    public bool Remove(string eventName, EventHandle handle)
    {
        EventDelegate del = this.GetEvent(eventName, false);
        if (del != null)
        {
            return del.Remove(handle);
        }
        return false;
    }

    public int RemoveListener(string eventName, Action callback)
    {
        EventDelegate del = this.GetEvent(eventName, false);
        if (del != null)
        {
            return del.RemoveListener(callback);
        }
        return 0;
    }

    public void RemoveAllListeners(string eventName)
    {
        this.GetEvent(eventName, false)?.RemoveAllListeners();
    }

    public void Invoke(string eventName)
    {
        this.GetEvent(eventName, false)?.Invoke();
    }

    public void InvokeAll(string eventName)
    {
        this.GetEvent(eventName, false)?.InvokeAll();
    }

    public void Clear()
    {
        foreach (var del in _mappings.Values)
        {
            del.RemoveAllListeners();
        }
        _mappings.Clear();
    }

    private EventDelegate GetEvent(string eventName, bool createIfInexist)
    {
        if (eventName.IsNullOrEmpty())
        {
            throw new ArgumentNullException($"¡¾{eventName}¡¿²»ÄÜÎª¿Õ£¡");
        }

        if (_mappings.TryGetValue(eventName, out EventDelegate handler) && createIfInexist)
        {
            return handler;
        }

        _mappings[eventName] = new EventDelegate();
        return _mappings[eventName];
    }
}
