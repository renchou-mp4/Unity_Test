namespace Managers
{
    public static class LogTools
    {
        public static void Log(string message)
        {
            LogManager._Instance._CurLogging.Log(new LogData
            {
                _OutputLevel = OutputLevel.Log,
                _Message = message
            });
        }

        public static void LogWarning(string message)
        {
            LogManager._Instance._CurLogging.Log(new LogData
            {
                _OutputLevel = OutputLevel.Warning,
                _Message = message
            });
        }

        public static void LogError(string message)
        {
            LogManager._Instance._CurLogging.Log(new LogData
            {
                _OutputLevel = OutputLevel.Error,
                _Message = message
            });
        }

        public static void LogFormat(string message, params object[] arguments)
        {
            LogManager._Instance._CurLogging.LogFormat(new LogData
            {
                _OutputLevel = OutputLevel.Log,
                _Message = message
            }, arguments);
        }

        public static void LogWarningFormat(string message, params object[] arguments)
        {
            LogManager._Instance._CurLogging.LogFormat(new LogData
            {
                _OutputLevel = OutputLevel.Warning,
                _Message = message
            }, arguments);
        }

        public static void LogErrorFormat(string message, params object[] arguments)
        {
            LogManager._Instance._CurLogging.LogFormat(new LogData
            {
                _OutputLevel = OutputLevel.Error,
                _Message = message
            }, arguments);
        }
    }

}

