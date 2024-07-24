using System.Collections.Generic;


public delegate void EventCallback(params object[] args);

//使用struct可以减少一些GC
public struct EventData
{
    private object _source;             //事件源
    private string _eventName;
    private EventCallback _callback;
    private object[] _callbackArguments;


    public string EventName { get => _eventName; set => _eventName = value; }

    public object Source { get => _source; set => _source = value; }

    public EventCallback Callback { get => _callback; set => _callback = value; }

    public object[] CallbackArgumants { get => _callbackArguments; set => _callbackArguments = value; }

    //public EventData(string eventName,
    //    object source = null,
    //    EventCallback callback = null)
    //{
    //    _eventName = eventName;
    //    _source = source;
    //    _callback = callback;
    //    _callbackArguments = null;
    //}

    public EventData(string eventName,
        object source = null,
        EventCallback callback = null,
        params object[] callbackArguments)
    {
        _eventName = eventName;
        _source = source;
        _callback = callback;
        _callbackArguments = callbackArguments;
    }

    public static EventData Add(string eventName, object source, EventCallback callback)
    {
        return new EventData(eventName, source, callback);
    }

    public static EventData Remove(string eventName, object source, EventCallback callback = null)
    {
        return new EventData(eventName, source, callback);
    }

    public static EventData Dispatch(string eventName, object source, params object[] callbackArguments)
    {
        return new EventData(eventName, source, null, callbackArguments);
    }
}


public class EventManager : MonoSingleton<EventManager>
{
    private const string _globalName = "Global";

    //<事件名,<事件源,<回调方法名，回调方法>>>
    private static Dictionary<string, Dictionary<object, Dictionary<int, EventCallback>>> _eventDic = new();


    public static void AddEvent(EventData eventData)
    {
        if (eventData.Source == null)
        {
            eventData.Source = _globalName;
        }

        if (_eventDic.ContainsKey(eventData.EventName))
        {
            if (_eventDic[eventData.EventName].ContainsKey(eventData.Source))
            {
                int hashCode = eventData.Callback.GetHashCode();
                if (_eventDic[eventData.EventName][eventData.Source].ContainsKey(hashCode))
                {
                    //ToDo 输出error，当前方法已注册
                    return;
                }
                else
                {
                    _eventDic[eventData.EventName][eventData.Source].Add(hashCode, eventData.Callback);
                }
            }
            else
            {
                _eventDic[eventData.EventName].Add(eventData.Source, new Dictionary<int, EventCallback>
                {
                    {eventData.Callback.GetHashCode(),eventData.Callback},
                });
            }
        }
        else
        {
            _eventDic.Add(eventData.EventName, new Dictionary<object, Dictionary<int, EventCallback>>
            {
                {eventData.Source, new Dictionary<int, EventCallback>
                {
                    {eventData.Callback.GetHashCode(),eventData.Callback },
                } },
            });
        }
    }


    public static void RemoveEvent(EventData eventData)
    {
        if (eventData.EventName == null)
        {
            eventData.EventName = _globalName;
        }

        if (!_eventDic.ContainsKey(eventData.EventName))
        {
            //ToDo 输出error，eventData.EventName没有注册
            return;
        }


        if (eventData.Source != null)
        {
            if (eventData.Callback == null)
            {
                //移除指定事件指定事件源的所有回调方法
                foreach (var callbackDic in _eventDic[eventData.EventName].Values)
                {
                    //主动释放内存，在内存受限时更好
                    callbackDic.Clear();
                }

                _eventDic[eventData.EventName].Remove(eventData.Source);

                if (_eventDic[eventData.EventName].Count == 0)
                {
                    _eventDic.Remove(eventData.EventName);
                }
            }
            else
            {
                //移除指定事件指定事件源的指定回调方法
                int hashCode = eventData.Callback.GetHashCode();

                if (!_eventDic[eventData.EventName][eventData.Source].ContainsKey(hashCode))
                {
                    //ToDo 输出error，eventData.CallbackName没有注册
                    return;
                }

                _eventDic[eventData.EventName][eventData.Source].Remove(hashCode);

                if (_eventDic[eventData.EventName][eventData.Source].Count == 0)
                {
                    _eventDic[eventData.EventName].Remove(eventData.Source);
                    if (_eventDic[eventData.EventName].Count == 0)
                    {
                        _eventDic.Remove(eventData.EventName);
                    }
                }
            }

        }
        else
        {
            //移除指定事件所有事件源
            foreach (var sourceDic in _eventDic.Values)
            {
                foreach (var callbackDic in sourceDic.Values)
                {
                    //主动释放内存，在内存受限时更好
                    callbackDic.Clear();
                }
                sourceDic.Clear();
            }
            _eventDic.Remove(eventData.EventName);
        }
    }


    public static void DispatchEvent(EventData eventData)
    {
        if (!_eventDic.ContainsKey(eventData.EventName))
        {
            //ToDo 输出error，eventData.EventName没有注册
            return;
        }

        if (eventData.Source == null)
        {
            eventData.Source = _globalName;

            //不指定事件源
            foreach (var sourceDic in _eventDic[eventData.EventName])
            {
                foreach (var callbackDic in sourceDic.Value)
                {
                    callbackDic.Value?.Invoke(eventData.CallbackArgumants);
                }
            }
        }
        else
        {
            //指定事件源
            foreach (var sourceDic in _eventDic[eventData.EventName])
            {
                if (eventData.Source == sourceDic.Key)
                {
                    foreach (var callbackDic in sourceDic.Value)
                    {
                        callbackDic.Value?.Invoke(eventData.CallbackArgumants);
                    }
                }
            }
        }
    }
}
