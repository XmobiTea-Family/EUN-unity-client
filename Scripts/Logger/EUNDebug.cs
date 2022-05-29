namespace XmobiTea.EUN.Logger
{
    using System;
    using UnityEngine;

    public enum LogType
    {
        Off,
        Exception,
        Error,
        Warning,
        All
    }

    /// <summary>
    /// Debug for EUN
    /// </summary>
    internal class EUNDebug
    {
        private static LogType logType;

        public static void Init(LogType logType)
        {
            EUNDebug.logType = logType;
        }

        public static void Log(object message)
        {
            if (logType >= LogType.All)
                Debug.Log(message);
        }

        public static void LogException(Exception exception)
        {
            if (logType >= LogType.Exception)
                Debug.LogException(exception);
        }

        public static void LogWarning(object message)
        {
            if (logType >= LogType.Warning)
                Debug.LogWarning(message);
        }

        public static void LogError(object message)
        {
            if (logType >= LogType.Error)
                Debug.LogError(message);
        }
    }
}
