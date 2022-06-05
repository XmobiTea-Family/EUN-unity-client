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
        private static LogType _logType;
        public static LogType logType
        {
            get => _logType;
            set => _logType = value;
        }

        public static void Log(object message)
        {
            if (_logType >= LogType.All)
                Debug.Log(message);
        }

        public static void LogException(Exception exception)
        {
            if (_logType >= LogType.Exception)
                Debug.LogException(exception);
        }

        public static void LogWarning(object message)
        {
            if (_logType >= LogType.Warning)
                Debug.LogWarning(message);
        }

        public static void LogError(object message)
        {
            if (_logType >= LogType.Error)
                Debug.LogError(message);
        }
    }
}
