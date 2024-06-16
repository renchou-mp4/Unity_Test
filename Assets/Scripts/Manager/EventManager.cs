using System;
using System.Collections.Generic;


public delegate void EventCallback(params object[] args);

public class EventData
{
    private object _source;
    private string _eventName;
    private string _callbackName;
    private EventCallback _callback;


    public string EventName { get => _eventName; set => _eventName = value; }
    /// <summary>
    /// 事件源，若为空，认为是全局事件，所有监听者都可以接收到消息
    /// </summary>
    public object Source { get => _source; set => _source = value; }
    public string CallbackName { get => _callbackName; set => _callbackName = value; }
    public EventCallback Callback { get => _callback; set => _callback = value; }

    public EventData(string eventName, object source, string callbackName, EventCallback callback)
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

    private static Dictionary<string, Dictionary<object, Dictionary<string, EventCallback>>> _eventDic = new();

    /// <summary>
    /// 添加事件
    /// </summary>
    /// <param name="eventName">事件名称</param>
    /// <param name="guid">事件源，使用Guid作为唯一标识符</param>
    /// <param name="callback">事件回调</param>
    public static void AddEvent(EventData eventData)
    {
        if (eventData.Source == null) eventData.Source = _globalName;

        //当前已包含该事件
        if (_eventDic.ContainsKey(eventData.EventName))
        {
            //当前事件已包含该事件源
            if (_eventDic[eventData.EventName].ContainsKey(eventData.Source))
            {
                //当前事件的该事件源已包含该回调函数
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

    public static void RemoveEvent(string eventName, Guid guid, EventCallback eventCallback)
    {
        if (!_eventDic.ContainsKey(eventName))
        {
            //ToDo 输出error
            return;
        }

        if (!_eventDic[eventName].ContainsKey(guid))
        {
            //ToDo 输出error
            return;
        }

        _eventDic[eventName].Remove(guid);

        if (_eventDic[eventName] == null)
        {
            _eventDic.Remove(eventName);
        }

        eventCallback?.Invoke(null);
    }

    public static void RemoveEvent(string eventName)
    {
        if (!_eventDic.ContainsKey(eventName))
        {
            //ToDo 输出error
        }
        else
        {
            _eventDic.Remove(eventName);
        }
    }

    public static void DispatchEvent(string eventName, Guid? guid, params object[] args)
    {
        if (String.IsNullOrEmpty(eventName))
        {
            eventName = _globalName;
        }
        if (!_eventDic.ContainsKey(eventName))
        {
            //ToDo 输出error
            return;
        }

        foreach (var item in _eventDic[eventName])
        {
            //item.Value.Invoke(args);
        }
    }
}
