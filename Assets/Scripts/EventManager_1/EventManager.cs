using System.Collections.Generic;


public delegate void EventCallback(params object[] args);

//使用struct可以减少一些GC
public struct EventData
{
    private object _source;             //事件源，若为空，认为是全局事件，所有监听者都可以接收到消息
    private string _eventName;
    private string _callbackName;
    private EventCallback _callback;


    public string EventName { get => _eventName; set => _eventName = value; }

    public object Source { get => _source; set => _source = value; }

    public string CallbackName { get => _callbackName; set => _callbackName = value; }

    public EventCallback Callback { get => _callback; set => _callback = value; }

    public EventData(string eventName, object source = null, string callbackName = null, EventCallback callback = null)
    {
        _eventName = eventName;
        _source = source;
        _callbackName = callbackName;
        _callback = callback;
    }
}


public class EventManager : MonoSingleton<EventManager>
{
    private const string _globalName = "Global";

    //<事件名,<事件源,<回调方法名，回调方法>>>
    private static Dictionary<string, Dictionary<object, Dictionary<string, EventCallback>>> _eventDic = new();


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
                if (_eventDic[eventData.EventName][eventData.Source].ContainsKey(eventData.CallbackName))
                {
                    //ToDo 输出error，当前方法已注册
                }
                else
                {
                    _eventDic[eventData.EventName][eventData.Source].Add(eventData.CallbackName, eventData.Callback);
                }
            }
            else
            {
                _eventDic[eventData.EventName].Add(eventData.Source, new Dictionary<string, EventCallback>
                {
                    {eventData.CallbackName,eventData.Callback},
                });
            }
        }
        else
        {
            _eventDic.Add(eventData.EventName, new Dictionary<object, Dictionary<string, EventCallback>>
            {
                {eventData.Source, new Dictionary<string, EventCallback>
                {
                    {eventData.CallbackName,eventData.Callback },
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
            if (!_eventDic[eventData.EventName].ContainsKey(eventData.Source))
            {
                //ToDo 输出error，eventData.Source没有注册
                return;
            }
            else
            {
                //移除指定事件源的指定回调方法
                if (!_eventDic[eventData.EventName][eventData.Source].ContainsKey(eventData.CallbackName))
                {
                    //ToDo 输出error，eventData.CallbackName没有注册
                    return;
                }
                else
                {
                    _eventDic[eventData.EventName][eventData.Source].Remove(eventData.CallbackName);

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
        }
        else
        {
            //移除所有事件源
        }


        _eventDic[eventData.EventName][eventData.Source].Remove(eventData.CallbackName);

        if (_eventDic[eventData.EventName][eventData.Source].Count == 0)
        {
            _eventDic[eventData.EventName].Remove(eventData.Source);
            if (_eventDic[eventData.EventName].Count == 0)
            {
                _eventDic.Remove(eventData.EventName);
            }
        }

        //执行移除事件回调
        eventData.Callback?.Invoke(null);
    }


    public static void DispatchEvent(EventData eventData, params object[] args)
    {
        if (eventData.Source == null)
        {
            eventData.Source = _globalName;
        }

        if (!_eventDic.ContainsKey(eventData.EventName))
        {
            //ToDo 输出error，eventData.EventName没有注册
            return;
        }

        if (eventData.Source != null)
        {
            //不指定事件源
            foreach (var sourceItem in _eventDic[eventData.EventName])
            {
                foreach (var eventItem in sourceItem.Value)
                {
                    eventItem.Value?.Invoke(args);
                }
            }
        }
        else
        {
            //指定事件源
            foreach (var sourceItem in _eventDic[eventData.EventName])
            {
                if (eventData.Source == sourceItem.Key)
                {
                    foreach (var eventItem in sourceItem.Value)
                    {
                        eventItem.Value?.Invoke(args);
                    }
                }
            }
        }
    }
}
