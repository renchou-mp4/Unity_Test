namespace Managers
{
    public interface ILogging
    {
        public void Log(LogData logData);

        public void LogFormat(LogData logData, params object[] arguments);
    }
}