using Managers;

public interface ILogging
{
    public abstract void Log(LogData logData);
    public abstract void LogFormat(LogData logData, params object[] arguments);
}
