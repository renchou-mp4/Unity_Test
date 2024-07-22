using System.Collections.Generic;


public delegate void EventCallback(params object[] args);

//ʹ��struct���Լ���һЩGC
public struct EventData
{
    private object _source;             //�¼�Դ����Ϊ�գ���Ϊ��ȫ���¼������м����߶����Խ��յ���Ϣ
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

    //<�¼���,<�¼�Դ,<�ص����������ص�����>>>
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
                    //ToDo ���error����ǰ������ע��
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
            //ToDo ���error��eventData.EventNameû��ע��
            return;
        }


        if (eventData.Source != null)
        {
            if (!_eventDic[eventData.EventName].ContainsKey(eventData.Source))
            {
                //ToDo ���error��eventData.Sourceû��ע��
                return;
            }
            else
            {
                //�Ƴ�ָ���¼�Դ��ָ���ص�����
                if (!_eventDic[eventData.EventName][eventData.Source].ContainsKey(eventData.CallbackName))
                {
                    //ToDo ���error��eventData.CallbackNameû��ע��
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
            //�Ƴ������¼�Դ
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

        //ִ���Ƴ��¼��ص�
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
            //ToDo ���error��eventData.EventNameû��ע��
            return;
        }

        if (eventData.Source != null)
        {
            //��ָ���¼�Դ
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
            //ָ���¼�Դ
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
