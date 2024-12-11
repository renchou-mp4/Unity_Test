using System.Collections.Generic;
using Tools;

namespace Managers
{
    /// <summary>
    /// 输出类型
    /// </summary>
    public enum OutputType
    {
        File,
        Console,
    }

    /// <summary>
    /// 输出级别
    /// </summary>
    public enum OutputLevel
    {
        Log,
        Warning,
        Error,
    }

    public class LogManager : MonoSingleton<LogManager>
    {
        private long _logIDCount = 0;

        public long _LogIDCount
        {
            get => ++_logIDCount;
            set => _logIDCount = value;
        }
        
        /// <summary>
        /// 日志开关
        /// </summary>
        public bool     _LogHandle  { get; set; }         = true;
        public ILogging _CurLogging { get; private set; } = new LogUnity();

        private readonly Dictionary<OutputType, ILogging> _logDic = new()
        {
            { OutputType.Console, new LogUnity() }
        };


        private delegate void LogDelegate(string message);
        
        public void ChangeOutputType(OutputType outputType)
        {
            if (_logDic.TryGetValue(outputType, out var value))
            {
                _CurLogging = value;
            }
            else
            {
                LogTools.LogWarning($"当前没有【{nameof(outputType)}】输出类型！");
            }
        }

        public void ChangeOutputLogging(OutputType outputType, ILogging logging)
        {
            if (_logDic.ContainsKey(outputType))
            {
                _logDic[outputType] = logging;
            }
            else
            {
                _logDic.Add(outputType, logging);
            }

            _CurLogging = _logDic[outputType];
        }
    }
}