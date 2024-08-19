using System.Collections.Generic;

public delegate void EventCallback(params object[] args);


public class EventManager : MonoSingleton<EventManager>
{
    private const string _globalName = "Global";

    //<�¼���,<�¼�Դ,<�ص����������ص�����>>>
    private static Dictionary<string, Dictionary<object, Dictionary<int, EventCallback>>> _eventDic = new();

    public static void AddEvent(object source, string eventName, EventCallback eventCallback)
    {
        if (source == null)
        {
            source = _globalName;
        }

        if (_eventDic.ContainsKey(eventName))
        {
            if (_eventDic[eventName].ContainsKey(source))
            {
                int hashCode = eventCallback.GetHashCode();
                if (_eventDic[eventName][source].ContainsKey(hashCode))
                {
                    LogManager.LogError($"��{eventName}���¼�---��{source}��Դ����ǰ������{eventCallback.Method.Name}����ע�ᣡ");
                    return;
                }
                else
                {
                    _eventDic[eventName][source].Add(hashCode, eventCallback);
                }
            }
            else
            {
                _eventDic[eventName].Add(source, new Dictionary<int, EventCallback>
                {
                    {eventCallback.GetHashCode(),eventCallback},
                });
            }
        }
        else
        {
            _eventDic.Add(eventName, new Dictionary<object, Dictionary<int, EventCallback>>
            {
                {source, new Dictionary<int, EventCallback>
                {
                    {eventCallback.GetHashCode(),eventCallback },
                } },
            });
        }
    }


    public static void RemoveEvent(object source, string eventName, EventCallback eventCallback = null)
    {
        if (eventName == null)
        {
            eventName = _globalName;
        }

        if (!_eventDic.ContainsKey(eventName))
        {
            LogManager.LogError($"�Ƴ�ʧ�ܣ���{eventName}���¼�û��ע�ᣡ");
            return;
        }


        if (source != null)
        {
            if (eventCallback == null)
            {
                //�Ƴ�ָ���¼�ָ���¼�Դ�����лص�����
                foreach (var callbackDic in _eventDic[eventName].Values)
                {
                    //�����ͷ��ڴ棬���ڴ�����ʱ����
                    callbackDic.Clear();
                }

                _eventDic[eventName].Remove(source);

                if (_eventDic[eventName].Count == 0)
                {
                    _eventDic.Remove(eventName);
                }
            }
            else
            {
                //�Ƴ�ָ���¼�ָ���¼�Դ��ָ���ص�����
                int hashCode = eventCallback.GetHashCode();

                if (!_eventDic[eventName][source].ContainsKey(hashCode))
                {
                    LogManager.LogError($"�Ƴ�ʧ�ܣ���{eventName}���¼�---��{source}��Դ����ǰ������{eventCallback.Method.Name}��û��ע�ᣡ");
                    return;
                }

                _eventDic[eventName][source].Remove(hashCode);

                if (_eventDic[eventName][source].Count == 0)
                {
                    _eventDic[eventName].Remove(source);
                    if (_eventDic[eventName].Count == 0)
                    {
                        _eventDic.Remove(eventName);
                    }
                }
            }

        }
        else
        {
            //�Ƴ�ָ���¼������¼�Դ
            foreach (var sourceDic in _eventDic.Values)
            {
                foreach (var callbackDic in sourceDic.Values)
                {
                    //�����ͷ��ڴ棬���ڴ�����ʱ����
                    callbackDic.Clear();
                }
                sourceDic.Clear();
            }
            _eventDic.Remove(eventName);
        }
    }


    public static void DispatchEvent(object source, string eventName, params object[] eventCallbackArguments)
    {
        if (!_eventDic.ContainsKey(eventName))
        {
            LogManager.LogError($"����ʧ�ܣ���{eventName}���¼�û��ע�ᣡ");
            return;
        }

        if (source == null)
        {
            source = _globalName;

            //��ָ���¼�Դ
            foreach (var sourceDic in _eventDic[eventName])
            {
                foreach (var callbackDic in sourceDic.Value)
                {
                    callbackDic.Value?.Invoke(eventCallbackArguments);
                }
            }
        }
        else
        {
            //ָ���¼�Դ
            foreach (var sourceDic in _eventDic[eventName])
            {
                if (source == sourceDic.Key)
                {
                    foreach (var callbackDic in sourceDic.Value)
                    {
                        callbackDic.Value?.Invoke(eventCallbackArguments);
                    }
                }
            }
        }
    }
}
