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
