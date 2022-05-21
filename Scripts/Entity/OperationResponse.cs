namespace XmobiTea.EUN.Entity
{
    using XmobiTea.EUN.Constant;

    using System.Text;

    using UnityEngine;
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Helper;

    public class OperationResponse
    {
        private int operationCode;
        private int returnCode;
        private string debugMessage;
        private EUNHashtable parameters;

        private int responseId;

        private float executeTime;

        public int GetOperationCode()
        {
            return operationCode;
        }

        public int GetReturnCode()
        {
            return returnCode;
        }

        public EUNHashtable GetParameters()
        {
            return parameters;
        }

        public int GetResponseId()
        {
            return responseId;
        }

        public string GetDebugMessage()
        {
            return debugMessage;
        }

        public bool HasError => GetReturnCode() != ReturnCode.Ok;

        public OperationResponse(OperationRequest operationRequest, int returnCode, string debugMessage, EUNHashtable parameters)
        {
            this.operationCode = (int)operationRequest.GetOperationCode();
            this.returnCode = returnCode;
            this.debugMessage = debugMessage;
            this.parameters = parameters;
            this.responseId = operationRequest.GetRequestId();
            this.executeTime = (Time.time - (operationRequest.GetEndTimeOut() - operationRequest.GetTimeOut())) * 1000;
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append("Code: " + CodeHelper.GetOperationCodeName(operationCode) + " ExecuteTime " + executeTime + "ms ResponseId: " + responseId + " ReturnCode " + CodeHelper.GetReturnCodeName(returnCode));

            if (returnCode == ReturnCode.Ok) stringBuilder.Append(" Parameters " + parameters);
            else stringBuilder.Append(" DebugMessage " + debugMessage);

            return stringBuilder.ToString();
        }
    }
}
