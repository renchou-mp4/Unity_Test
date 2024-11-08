using System.Collections.Generic;
using Tools;

namespace Managers
{
    public enum OutputType
    {
        File,
        Console,
    }

    public enum OutputLevel
    {
        Log,
        Warning,
        Error,
    }

    public struct LogData
    {
        public long _ID { get; set; }
        public string _Message { get; set; }
        public OutputLevel _OutputLevel { get; set; }

        public LogData(OutputLevel outputLevel, string message)
        {
            _ID = LogManager._Instance._LogIDCount;
            _Message = message;
            _OutputLevel = outputLevel;
        }
    }

    public class LogManager : MonoSingleton<LogManager>
    {
        private long _logIDCount = 0;
        public long _LogIDCount
        {
            get => ++_logIDCount;
            set => _logIDCount = value;
        }
        public bool _LogSwitch { get; set; } = true;


        public ILogging _CurLogging { get; set; } = new LogUnity();
        private Dictionary<OutputType, ILogging> _logDic = new()
        {
            {OutputType.Console, new LogUnity() }
        };


        private delegate void LogDelegate(string message);


        public void ChangeOutputType(OutputType outputType)
        {
            if (_logDic.ContainsKey(outputType))
            {
                _CurLogging = _logDic[outputType];
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
