using System.Collections.Generic;

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
        _ID = LogManager._LogIDCount;
        _Message = message;
        _OutputLevel = outputLevel;
    }
}

public class LogManager : MonoSingleton<LogManager>
{
    private static long _logIDCount = 0;
    public static long _LogIDCount
    {
        get => ++_logIDCount;
        set => _logIDCount = value;
    }
    public static bool _LogSwitch { get; set; } = true;

    private static ILogging _curLogging;
    private static Dictionary<OutputType, ILogging> _logDic = new();
    private delegate void LogDelegate(string message);

    protected override void SingletonAwake()
    {
        base.SingletonAwake();
        _logDic.Clear();
        _logDic.Add(OutputType.Console, new LogUnity());
        _curLogging = _logDic[OutputType.Console];
    }

    public static void Log(string message)
    {
        _curLogging.Log(new LogData
        {
            _OutputLevel = OutputLevel.Log,
            _Message = message
        });
    }

    public static void LogWarning(string message)
    {
        _curLogging.Log(new LogData
        {
            _OutputLevel = OutputLevel.Warning,
            _Message = message
        });
    }

    public static void LogError(string message)
    {
        _curLogging.Log(new LogData
        {
            _OutputLevel = OutputLevel.Error,
            _Message = message
        });
    }

    public static void LogFormat(string message, params object[] arguments)
    {
        _curLogging.LogFormat(new LogData
        {
            _OutputLevel = OutputLevel.Log,
            _Message = message
        }, arguments);
    }

    public static void LogWarningFormat(string message, params object[] arguments)
    {
        _curLogging.LogFormat(new LogData
        {
            _OutputLevel = OutputLevel.Warning,
            _Message = message
        }, arguments);
    }

    public static void LogErrorFormat(string message, params object[] arguments)
    {
        _curLogging.LogFormat(new LogData
        {
            _OutputLevel = OutputLevel.Error,
            _Message = message
        }, arguments);
    }

    public static void ChangeOutputType(OutputType outputType)
    {
        if (_logDic.ContainsKey(outputType))
        {
            _curLogging = _logDic[outputType];
        }
        else
        {
            LogWarning($"当前没有【{nameof(outputType)}】输出类型！");
        }
    }

    public static void ChangeOutputLogging(OutputType outputType, ILogging logging)
    {
        if (_logDic.ContainsKey(outputType))
        {
            _logDic[outputType] = logging;
        }
        else
        {
            _logDic.Add(outputType, logging);
        }
        _curLogging = _logDic[outputType];
    }
}
