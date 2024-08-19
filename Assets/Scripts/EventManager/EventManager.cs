using System.Collections.Generic;

public delegate void EventCallback(params object[] args);


public class EventManager : MonoSingleton<EventManager>
{
    private const string _globalName = "Global";

    //<事件名,<事件源,<回调方法名，回调方法>>>
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
                    LogManager.LogError($"【{eventName}】事件---【{source}】源：当前方法【{eventCallback.Method.Name}】已注册！");
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
            LogManager.LogError($"移除失败，【{eventName}】事件没有注册！");
            return;
        }


        if (source != null)
        {
            if (eventCallback == null)
            {
                //移除指定事件指定事件源的所有回调方法
                foreach (var callbackDic in _eventDic[eventName].Values)
                {
                    //主动释放内存，在内存受限时更好
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
                //移除指定事件指定事件源的指定回调方法
                int hashCode = eventCallback.GetHashCode();

                if (!_eventDic[eventName][source].ContainsKey(hashCode))
                {
                    LogManager.LogError($"移除失败，【{eventName}】事件---【{source}】源：当前方法【{eventCallback.Method.Name}】没有注册！");
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
            _eventDic.Remove(eventName);
        }
    }


    public static void DispatchEvent(object source, string eventName, params object[] eventCallbackArguments)
    {
        if (!_eventDic.ContainsKey(eventName))
        {
            LogManager.LogError($"调用失败，【{eventName}】事件没有注册！");
            return;
        }

        if (source == null)
        {
            source = _globalName;

            //不指定事件源
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
            //指定事件源
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
