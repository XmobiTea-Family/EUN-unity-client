namespace XmobiTea.EUN.Helper
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using XmobiTea.EUN.Constant;

    public class CodeHelper
    {
        private const string UnknownCode = "Unknown";

        private static Dictionary<int, string> operationCodeDict;
        private static Dictionary<int, string> eventCodeDict;
        private static Dictionary<int, string> returnCodeDict;

        /// <summary>
        /// Get operation code name via id
        /// </summary>
        /// <param name="operationCode"></param>
        /// <returns></returns>
        public static string getOperationCodeName(int operationCode)
        {
            return operationCodeDict.ContainsKey(operationCode) ? operationCodeDict[operationCode] : UnknownCode;
        }

        /// <summary>
        /// Get event code name via id
        /// </summary>
        /// <param name="eventCode"></param>
        /// <returns></returns>
        public static string getEventCodeName(int eventCode)
        {
            return eventCodeDict.ContainsKey(eventCode) ? eventCodeDict[eventCode] : UnknownCode;
        }

        /// <summary>
        /// Get return code name via id
        /// </summary>
        /// <param name="returnCode"></param>
        /// <returns></returns>
        public static string getReturnCodeName(int returnCode)
        {
            return returnCodeDict.ContainsKey(returnCode) ? returnCodeDict[returnCode] : UnknownCode;
        }

        static CodeHelper()
        {
            setOperationCodeDict();
            setEventCodeDict();
            setReturnCodeDict();
        }

        private static void setOperationCodeDict()
        {
            operationCodeDict = new Dictionary<int, string>();

            var fields = typeof(OperationCode).GetFields(BindingFlags.Public | BindingFlags.Static);

            foreach (var field in fields)
            {
                operationCodeDict[Convert.ToInt32(field.GetValue(null))] = field.Name;
            }
        }

        private static void setEventCodeDict()
        {
            eventCodeDict = new Dictionary<int, string>();

            var fields = typeof(EventCode).GetFields(BindingFlags.Public | BindingFlags.Static);

            foreach (var field in fields)
            {
                eventCodeDict[Convert.ToInt32(field.GetValue(null))] = field.Name;
            }
        }

        private static void setReturnCodeDict()
        {
            returnCodeDict = new Dictionary<int, string>();

            var fields = typeof(ReturnCode).GetFields(BindingFlags.Public | BindingFlags.Static);

            foreach (var field in fields)
            {
                returnCodeDict[Convert.ToInt32(field.GetValue(null))] = field.Name;
            }
        }

    }

}
