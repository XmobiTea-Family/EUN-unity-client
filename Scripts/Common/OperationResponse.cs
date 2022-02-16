namespace EUN.Common
{
    using EUN.Constant;

    using System.Text;

    using UnityEngine;

    public class OperationResponse
    {
        private int operationCode;
        private int returnCode;
        private string debugMessage;
        private CustomHashtable parameters;

        private int responseId;

        private float executeTime;

        public OperationCode GetOperationCode()
        {
            return (OperationCode)operationCode;
        }

        public ReturnCode GetReturnCode()
        {
            return (ReturnCode)returnCode;
        }

        public CustomHashtable GetParameters()
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

        public OperationResponse(OperationRequest operationRequest, int returnCode, string debugMessage, CustomHashtable parameters)
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

            stringBuilder.Append("Code: " + GetOperationCode() + " ExecuteTime " + executeTime + "ms ResponseId: " + responseId + " ReturnCode " + GetReturnCode());

            return stringBuilder.ToString();
        }
    }
}