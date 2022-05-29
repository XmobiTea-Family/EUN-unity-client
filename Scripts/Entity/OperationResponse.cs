namespace XmobiTea.EUN.Entity
{
    using XmobiTea.EUN.Constant;

    using System.Text;

    using UnityEngine;
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Helper;

    /// <summary>
    /// The operation response
    /// </summary>
    public class OperationResponse
    {
        /// <summary>
        /// The operation code this response
        /// </summary>
        private int operationCode;

        /// <summary>
        /// The return code
        /// </summary>
        private int returnCode;

        /// <summary>
        /// The debug message from EUN Server
        /// </summary>
        private string debugMessage;

        /// <summary>
        /// The parameters EUN Server attactment
        /// </summary>
        private EUNHashtable parameters;

        /// <summary>
        /// The response id, it corresponding with request id in OperationRequest
        /// </summary>
        private int responseId;

        /// <summary>
        /// The execute time
        /// </summary>
        private float executeTime;

        /// <summary>
        /// The operation code
        /// </summary>
        /// <returns></returns>
        public int GetOperationCode()
        {
            return this.operationCode;
        }

        /// <summary>
        /// The return code
        /// </summary>
        /// <returns></returns>
        public int GetReturnCode()
        {
            return this.returnCode;
        }

        /// <summary>
        /// The parameters EUN Server attactment
        /// </summary>
        /// <returns></returns>
        public EUNHashtable GetParameters()
        {
            return this.parameters;
        }

        /// <summary>
        /// Get response id of this response
        /// </summary>
        /// <returns></returns>
        public int GetResponseId()
        {
            return this.responseId;
        }

        /// <summary>
        /// The debug message
        /// </summary>
        /// <returns></returns>
        public string GetDebugMessage()
        {
            return this.debugMessage;
        }

        /// <summary>
        /// This response has error from EUN Server or not
        /// </summary>
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

            stringBuilder.Append("Code: " + CodeHelper.GetOperationCodeName(this.operationCode) + " ExecuteTime " + this.executeTime + "ms ResponseId: " + this.responseId + " ReturnCode " + CodeHelper.GetReturnCodeName(this.returnCode));

            if (this.returnCode == ReturnCode.Ok) stringBuilder.Append(" Parameters " + this.parameters);
            else stringBuilder.Append(" DebugMessage " + this.debugMessage);

            return stringBuilder.ToString();
        }
    }
}
