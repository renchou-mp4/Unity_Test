using System.Collections.Generic;
using Tools;

namespace Managers
{
    public delegate void EventCallback(params object[] args);

    public class EventManager : MonoSingleton<EventManager>
    {
        private const string GLOBAL_NAME = "Global";

        //<事件名,<事件源,<回调方法哈希值，回调方法>>>
        private static readonly Dictionary<string, Dictionary<object, Dictionary<int, EventCallback>>> _eventDic = new();

        /// <summary>
        /// 添加事件
        /// </summary>
        /// <param name="source">事件源</param>
        /// <param name="eventName">事件名称</param>
        /// <param name="eventCallback">事件回调</param>
        public static void AddEvent(object source, string eventName, EventCallback eventCallback)
        {
            source ??= GLOBAL_NAME;

            //不包含该事件
            if (!_eventDic.ContainsKey(eventName))
            {
                _eventDic.Add(eventName, new Dictionary<object, Dictionary<int, EventCallback>>
                {
                    {
                        source, new Dictionary<int, EventCallback>
                        {
                            { eventCallback.GetHashCode(), eventCallback },
                        }
                    },
                });
                return;
            }

            //已包含该事件
            //不包含该事件源
            if (_eventDic[eventName].ContainsKey(source))
            {
                _eventDic[eventName].Add(source, new Dictionary<int, EventCallback>
                {
                    { eventCallback.GetHashCode(), eventCallback },
                });
                return;
            }

            //已包含该事件源
            //不包含该事件回调
            int hashCode = eventCallback.GetHashCode();
            if (!_eventDic[eventName][source].ContainsKey(hashCode))
            {
                _eventDic[eventName][source].Add(hashCode, eventCallback);
                return;
            }

            //已包含该事件回调
            LogTools.LogError($"【{eventName}】事件---【{source}】源：当前方法【{eventCallback.Method.Name}】已注册！");
        }

        /// <summary>
        /// 删除事件
        /// </summary>
        /// <param name="source">事件源</param>
        /// <param name="eventName">事件名称</param>
        /// <param name="eventCallback">事件回调</param>
        public static void RemoveEvent(object source, string eventName, EventCallback eventCallback = null)
        {
            eventName ??= GLOBAL_NAME;

            if (!_eventDic.ContainsKey(eventName))
            {
                LogTools.LogError($"移除失败，【{eventName}】事件没有注册！");
                return;
            }

            //事件源为空
            if (source == null)
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
                return;
            }
            
            //事件源不为空
            //指定事件回调不为空
            if (eventCallback is not null)
            {
                //移除指定事件指定事件源的指定事件回调
                int hashCode = eventCallback.GetHashCode();

                if (!_eventDic[eventName][source].ContainsKey(hashCode))
                {
                    LogTools.LogError($"移除失败，【{eventName}】事件---【{source}】源：当前方法【{eventCallback.Method.Name}】没有注册！");
                    return;
                }

                _eventDic[eventName][source].Remove(hashCode);

                //检测该事件及事件源是否已没有事件回调，没有则删除
                if (_eventDic[eventName][source].Count != 0) return;
                _eventDic[eventName].Remove(source);
                if (_eventDic[eventName].Count != 0) return;
                _eventDic.Remove(eventName);

                return;
            }

            //指定事件回调为空，移除指定事件指定事件源的所有事件回调
            foreach (var callbackDic in _eventDic[eventName].Values)
            {
                //主动释放内存，在内存受限时更好
                callbackDic.Clear();
            }
            _eventDic[eventName].Remove(source);
            
            //检测该事件的事件源是为0，是则删除事件
            if (_eventDic[eventName].Count != 0) return;
            _eventDic.Remove(eventName);
        }

        /// <summary>
        /// 调用事件
        /// </summary>
        /// <param name="source">事件源</param>
        /// <param name="eventName">事件名称</param>
        /// <param name="eventCallbackArguments">事件回调参数</param>
        public static void DispatchEvent(object source, string eventName, params object[] eventCallbackArguments)
        {
            if (!_eventDic.ContainsKey(eventName))
            {
                LogTools.LogError($"调用失败，【{eventName}】事件没有注册！");
                return;
            }

            //事件源不为空
            if (source is not null)
            {
                foreach (var sourceDic in _eventDic[eventName])
                {
                    if (source == sourceDic.Key)
                    {
                        foreach (var callbackDic in sourceDic.Value)
                        {
                            callbackDic.Value?.Invoke(eventCallbackArguments);
                        }
                        return;
                    }
                }
            }
            
            //事件源为空
            foreach (var sourceDic in _eventDic[eventName])
            {
                foreach (var callbackDic in sourceDic.Value)
                {
                    callbackDic.Value?.Invoke(eventCallbackArguments);
                }
            }
        }
    }
}