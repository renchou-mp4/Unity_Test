using System;
using UnityEngine;

namespace Managers
{
    public class LogUnity : ILogging
    {
        public void Log(LogData logData)
        {
            switch (logData._OutputLevel)
            {
                case OutputLevel.Log:
                    LogMessage(logData);
                    break;
                case OutputLevel.Warning:
                    LogWarning(logData);
                    break;
                case OutputLevel.Error:
                    LogError(logData);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void LogFormat(LogData logData, params object[] arguments)
        {
            switch (logData._OutputLevel)
            {
                case OutputLevel.Log:
                    LogMessageFormat(logData, arguments);
                    break;
                case OutputLevel.Warning:
                    LogWarningFormat(logData, arguments);
                    break;
                case OutputLevel.Error:
                    LogErrorFormat(logData, arguments);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static void LogMessage(LogData logData)
        {
            Debug.Log($"ID:{logData._ID}---{logData._Message}");
        }

        private static void LogWarning(LogData logData)
        {
            Debug.LogWarning($"ID:{logData._ID}---{logData._Message}");
        }

        private static void LogError(LogData logData)
        {
            Debug.LogError($"ID:{logData._ID}---{logData._Message}");
        }

        private static void LogMessageFormat(LogData logData, params object[] arguments)
        {
            Debug.LogFormat($"ID:{logData._ID}---{logData._Message}", arguments);
        }

        private static void LogWarningFormat(LogData logData, params object[] arguments)
        {
            Debug.LogWarningFormat($"ID:{logData._ID}---{logData._Message}", arguments);
        }
        
        private static void LogErrorFormat(LogData logData, params object[] arguments)
        {
            Debug.LogErrorFormat($"ID:{logData._ID}---{logData._Message}", arguments);
        }
    }
}
