using System.Collections.Generic;


public delegate void EventCallback(params object[] args);

public class EventData
{
    private object _source;
    private string _eventName;
    private string _callbackName;
    private EventCallback _callback;
    private bool _isDisposable;


    public string EventName { get => _eventName; set => _eventName = value; }
    /// <summary>
    /// �¼�Դ����Ϊ�գ���Ϊ��ȫ���¼������м����߶����Խ��յ���Ϣ
    /// </summary>
    public object Source { get => _source; set => _source = value; }
    public string CallbackName { get => _callbackName; set => _callbackName = value; }
    public EventCallback Callback { get => _callback; set => _callback = value; }
    public bool IsDisposable { get => _isDisposable; set => _isDisposable = value; }

    public EventData(string eventName, object source, string callbackName, EventCallback callback, bool isDisposable = false)
    {
        _eventName = eventName;
        _source = source;
        _callbackName = callbackName;
        _callback = callback;
        _isDisposable = isDisposable;
    }
}


public class EventManager : MonoSingleton<EventManager>
{
    private const string _globalName = "Global";

    private static Dictionary<string, Dictionary<object, Dictionary<string, EventCallback>>> _eventDic = new();

    /// <summary>
    /// ����¼�
    /// </summary>
    /// <param name="eventName">�¼�����</param>
    /// <param name="guid">�¼�Դ��ʹ��Guid��ΪΨһ��ʶ��</param>
    /// <param name="callback">�¼��ص�</param>
    public static void AddEvent(EventData eventData)
    {
        if (eventData.Source == null) eventData.Source = _globalName;

        //��ǰ�Ѱ������¼�
        if (_eventDic.ContainsKey(eventData.EventName))
        {
            //��ǰ�¼��Ѱ������¼�Դ
            if (_eventDic[eventData.EventName].ContainsKey(eventData.Source))
            {
                //��ǰ�¼��ĸ��¼�Դ�Ѱ����ûص�����
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
        if (!_eventDic.ContainsKey(eventData.EventName))
        {
            //ToDo ���error��eventData.EventNameû��ע��
            return;
        }

        if (!_eventDic[eventData.EventName].ContainsKey(eventData.Source))
        {
            //ToDo ���error��eventData.Sourceû��ע��
            return;
        }

        if (!_eventDic[eventData.EventName][eventData.Source].ContainsKey(eventData.CallbackName))
        {
            //ToDo ���error��eventData.CallbackNameû��ע��
            return;
        }

        _eventDic[eventData.EventName][eventData.Source].Remove(eventData.CallbackName);

        if (_eventDic[eventData.EventName][eventData.Source] == null)
        {
            _eventDic[eventData.EventName].Remove(eventData.Source);
            if (_eventDic[eventData.EventName] == null)
            {
                _eventDic.Remove(eventData.EventName);
            }
        }

        //ִ���Ƴ��¼��ص�
        eventData.Callback?.Invoke(null);
    }

    public static void RemoveEvent(string eventName, EventCallback eventCallback)
    {
        if (!_eventDic.ContainsKey(eventName))
        {
            //ToDo ���error��eventData.EventNameû��ע��
        }
        else
        {
            _eventDic.Remove(eventName);

            eventCallback?.Invoke(null);
        }
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

        foreach (var sourceItem in _eventDic[eventData.EventName])
        {
            foreach (var eventItem in sourceItem.Value)
            {
                eventItem.Value?.Invoke(args);
            }
        }
    }
}
