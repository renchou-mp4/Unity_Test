namespace Managers
{
    public static class LogTools
    {
        public static void Log(string message)
        {
            LogManager._Instance._CurLogging.Log(new LogData(OutputLevel.Log, message));
        }

        public static void LogWarning(string message)
        {
            LogManager._Instance._CurLogging.Log(new LogData(OutputLevel.Warning, message));
        }

        public static void LogError(string message)
        {
            LogManager._Instance._CurLogging.Log(new LogData(OutputLevel.Error, message));
        }

        public static void LogFormat(string message, params object[] arguments)
        {
            LogManager._Instance._CurLogging.LogFormat(new LogData(OutputLevel.Log, message), arguments);
        }

        public static void LogWarningFormat(string message, params object[] arguments)
        {
            LogManager._Instance._CurLogging.LogFormat(new LogData(OutputLevel.Warning, message), arguments);
        }

        public static void LogErrorFormat(string message, params object[] arguments)
        {
            LogManager._Instance._CurLogging.LogFormat(new LogData(OutputLevel.Error, message), arguments);
        }
    }
}