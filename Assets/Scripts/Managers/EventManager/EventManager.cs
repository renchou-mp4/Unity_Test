using System;
using System.Collections.Generic;
using System.Linq;

namespace Managers
{
    public struct EventBody : IEquatable<EventBody>
    {
        public int _HashCode {get;set;}
        public string _EventDesc { get; set; }
        public Delegate _EventDelegate { get; set; }

        public EventBody(string eventDesc, Delegate eventDelegate)
        {
            _HashCode = eventDelegate.GetHashCode();
            _EventDesc = eventDesc;
            _EventDelegate = eventDelegate;
        }

        public bool Equals(EventBody other)
        {
            return _HashCode == other._HashCode;
        }
    }
    
    public class EventManager : BaseManager<EventManager>
    {
        //<事件ID，事件体>
        private static readonly Dictionary<int, List<EventBody>> _eventDic = new();

        /// <summary>
        /// 添加事件
        /// </summary>
        /// <param name="eventID">事件ID</param>
        /// <param name="eventBody">事件体</param>
        public static void AddEvent(int eventID, EventBody eventBody)
        {
            //不包含该事件
            if (!_eventDic.TryGetValue(eventID, out var list))
            {
                _eventDic.Add(eventID, new List<EventBody>(){eventBody});
                return;
            }

            //已包含该事件
            //包含该事件回调
            if (list.Any(body => body._HashCode == eventBody._HashCode))
            {
                LogTools.LogError($"事件ID：【{eventID}】--- 当前方法【{eventBody._EventDelegate.Method.Name}】已注册！描述：{eventBody._EventDesc}");
                return;
            }

            //不包含该事件回调
            _eventDic[eventID].Add(eventBody);
        }

        /// <summary>
        /// 删除事件
        /// </summary>
        /// <param name="eventID">事件ID</param>
        /// <param name="eventBody">事件体</param>
        public static void RemoveEvent(int eventID, EventBody eventBody)
        {
            if (!_eventDic.TryGetValue(eventID,out var list))
            {
                LogTools.LogError($"移除失败，【{eventID}】事件没有注册！");
                return;
            }

            if (list.All(body => body._HashCode != eventBody._HashCode))
            {
                LogTools.LogError($"移除失败，【{eventID}】事件没有注册! 当前方法【{eventBody._EventDelegate.Method.Name}】已注册！描述：{eventBody._EventDesc}");
                return;
            }
            
            list.Remove(eventBody);
            
            //检测该事件及事件源是否已没有事件回调，没有则删除
            if (_eventDic[eventID].Count != 0) return;
            _eventDic.Remove(eventID);
        }

        /// <summary>
        /// 调用事件
        /// </summary>
        /// <param name="eventID">事件ID</param>
        public static void DispatchEvent(int eventID)
        {
            if (!_eventDic.TryGetValue(eventID,out var list))
            {
                LogTools.LogError($"调用失败，【{eventID}】事件没有注册！");
                return;
            }

            foreach (var body in list)
                body._EventDelegate.DynamicInvoke();
        }
        public static void DispatchEvent<T>(int eventID,T data)
        {
            if (!_eventDic.TryGetValue(eventID,out var list))
            {
                LogTools.LogError($"调用失败，【{eventID}】事件没有注册！");
                return;
            }

            foreach (var body in list)
                body._EventDelegate.DynamicInvoke(data);
        }
        public static void DispatchEvent<T1,T2>(int eventID,T1 data1,T2 data2)
        {
            if (!_eventDic.TryGetValue(eventID,out var list))
            {
                LogTools.LogError($"调用失败，【{eventID}】事件没有注册！");
                return;
            }

            foreach (var body in list)
                body._EventDelegate.DynamicInvoke(data1,data2);
        }
        public static void DispatchEvent<T1,T2,T3>(int eventID,T1 data1,T2 data2,T3 data3)
        {
            if (!_eventDic.TryGetValue(eventID,out var list))
            {
                LogTools.LogError($"调用失败，【{eventID}】事件没有注册！");
                return;
            }

            foreach (var body in list)
                body._EventDelegate.DynamicInvoke(data1,data2,data3);
        }
        public static void DispatchEvent<T1,T2,T3,T4>(int eventID,T1 data1,T2 data2,T3 data3,T4 data4)
        {
            if (!_eventDic.TryGetValue(eventID,out var list))
            {
                LogTools.LogError($"调用失败，【{eventID}】事件没有注册！");
                return;
            }

            foreach (var body in list)
                body._EventDelegate.DynamicInvoke(data1,data2,data3,data4);
        }
        public static void DispatchEvent<T1,T2,T3,T4,T5>(int eventID,T1 data1,T2 data2,T3 data3,T4 data4,T5 data5)
        {
            if (!_eventDic.TryGetValue(eventID,out var list))
            {
                LogTools.LogError($"调用失败，【{eventID}】事件没有注册！");
                return;
            }

            foreach (var body in list)
                body._EventDelegate.DynamicInvoke(data1,data2,data3,data4,data5);
        }
        
        //TODO:延时调用
    }
}