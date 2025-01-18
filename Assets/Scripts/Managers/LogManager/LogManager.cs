using System.Collections.Generic;

namespace Managers
{
    /// <summary>
    ///     输出类型
    /// </summary>
    public enum OutputType
    {
        File,
        Console
    }

    /// <summary>
    ///     输出级别
    /// </summary>
    public enum OutputLevel
    {
        Log,
        Warning,
        Error
    }

    public class LogManager : MonoSingleton<LogManager>
    {
        private readonly Dictionary<OutputType, ILogging> _logDic = new()
        {
            { OutputType.Console, new LogUnity() }
        };

        private long _logIDCount = 0;

        public long _LogIDCount
        {
            get => ++_logIDCount;
            set => _logIDCount = value;
        }

        /// <summary>
        ///     日志开关
        /// </summary>
        public bool _LogHandle { get; set; } = true;

        public ILogging _CurLogging { get; private set; } = new LogUnity();

        public void Start()
        {
            _logIDCount = 0;
        }
        
        /// <summary>
        /// 修改输出类型
        /// </summary>
        /// <param name="outputType"></param>
        public void ChangeOutputType(OutputType outputType)
        {
            if (_logDic.TryGetValue(outputType, out var value))
                _CurLogging = value;
            else
                LogTools.LogWarning($"当前没有【{nameof(outputType)}】输出类型！");
        }

        /// <summary>
        /// 修改输出logging
        /// </summary>
        /// <param name="outputType"></param>
        /// <param name="logging"></param>
        public void ChangeOutputLogging(OutputType outputType, ILogging logging)
        {
            if (_logDic.ContainsKey(outputType))
                _logDic[outputType] = logging;
            else
                _logDic.Add(outputType, logging);

            _CurLogging = _logDic[outputType];
        }
    }
}