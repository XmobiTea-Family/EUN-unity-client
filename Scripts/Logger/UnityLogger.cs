namespace XmobiTea.EUN.Logger
{
    using System;
    using UnityEngine;

    public class UnityLogger
#if EUN_USING_ONLINE
        : com.tvd12.ezyfoxserver.client.logger.EzyLogger
#endif
    {
        private readonly object type;

        public UnityLogger(object type)
        {
            this.type = type;
        }

        public void debug(string format, params object[] args)
        {
            Debug.Log(type + " - " + format);
        }

        public void debug(string message, Exception e)
        {
            Debug.Log(type + " - " + message + "\n" + e);
        }

        public void error(string format, params object[] args)
        {
            Debug.LogError(type + " - " + format);
        }

        public void error(string message, Exception e)
        {
            Debug.LogError(type + " - " + message + "\n" + e);
        }

        public void info(string format, params object[] args)
        {
            Debug.Log(type + " - " + format);
        }

        public void info(string message, Exception e)
        {
            Debug.Log(type + " - " + message + "\n" + e);
        }

        public void trace(string format, params object[] args)
        {
            Debug.Log(type + " - " + format);
        }

        public void trace(string message, Exception e)
        {
            Debug.Log(type + " - " + message + "\n" + e);
        }

        public void warn(string format, params object[] args)
        {
            Debug.LogWarning(type + " - " + format);
        }

        public void warn(string message, Exception e)
        {
            Debug.LogWarning(type + " - " + message + "\n" + e);
        }
    }
}
