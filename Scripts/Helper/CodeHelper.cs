using System;
using System.Collections.Generic;
using System.Reflection;

using XmobiTea.EUN.Constant;

namespace XmobiTea.EUN.Helper
{
    public class CodeHelper
    {
        private static string UnknownCode = "Unknown";

        private static Dictionary<int, string> operationCodeDic;
        private static Dictionary<int, string> eventCodeDic;
        private static Dictionary<int, string> returnCodeDic;

        public static string GetOperationCodeName(int operationCode)
        {
            return operationCodeDic.ContainsKey(operationCode) ? operationCodeDic[operationCode] : UnknownCode;
        }

        public static string GetEventCodeName(int eventCode)
        {
            return eventCodeDic.ContainsKey(eventCode) ? eventCodeDic[eventCode] : UnknownCode;
        }

        public static string GetReturnCodeName(int returnCode)
        {
            return returnCodeDic.ContainsKey(returnCode) ? returnCodeDic[returnCode] : UnknownCode;
        }

        static CodeHelper()
        {
            SetOperationCodeDic();
            SetEventCodeDic();
            SetReturnCodeDic();
        }

        private static void SetOperationCodeDic()
        {
            operationCodeDic = new Dictionary<int, string>();

            var fields = typeof(OperationCode).GetFields(BindingFlags.Public | BindingFlags.Static);

            foreach (var field in fields)
            {
                operationCodeDic[Convert.ToInt32(field.GetValue(null))] = field.Name;
            }
        }

        private static void SetEventCodeDic()
        {
            eventCodeDic = new Dictionary<int, string>();

            var fields = typeof(EventCode).GetFields(BindingFlags.Public | BindingFlags.Static);

            foreach (var field in fields)
            {
                eventCodeDic[Convert.ToInt32(field.GetValue(null))] = field.Name;
            }
        }

        private static void SetReturnCodeDic()
        {
            returnCodeDic = new Dictionary<int, string>();

            var fields = typeof(ReturnCode).GetFields(BindingFlags.Public | BindingFlags.Static);

            foreach (var field in fields)
            {
                returnCodeDic[Convert.ToInt32(field.GetValue(null))] = field.Name;
            }
        }
    }
}