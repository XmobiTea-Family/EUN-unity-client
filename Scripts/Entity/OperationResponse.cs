namespace XmobiTea.EUN.Entity
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;
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
        /// This response has encrypted or not
        /// </summary>
        private bool encrypted;

        /// <summary>
        /// The operation code
        /// </summary>
        /// <returns></returns>
        public int getOperationCode()
        {
            return this.operationCode;
        }

        /// <summary>
        /// The return code
        /// </summary>
        /// <returns></returns>
        public int getReturnCode()
        {
            return this.returnCode;
        }

        /// <summary>
        /// The parameters EUN Server attactment
        /// </summary>
        /// <returns></returns>
        public EUNHashtable getParameters()
        {
            return this.parameters;
        }

        /// <summary>
        /// Get response id of this response
        /// </summary>
        /// <returns></returns>
        public int getResponseId()
        {
            return this.responseId;
        }

        /// <summary>
        /// The debug message
        /// </summary>
        /// <returns></returns>
        public string getDebugMessage()
        {
            return this.debugMessage;
        }

        /// <summary>
        /// This response has error from EUN Server or not
        /// </summary>
        public bool hasError() => this.getReturnCode() != ReturnCode.Ok;

        /// <summary>
        /// Constructor for OperationResponse
        /// </summary>
        /// <param name="operationCode"></param>
        /// <param name="responseId"></param>
        /// <param name="encrypted"></param>
        public OperationResponse(int operationCode, int responseId, bool encrypted = true)
        {
            this.operationCode = operationCode;
            this.responseId = responseId;

            this.encrypted = encrypted;
        }

        /// <summary>
        /// Check this response has encrypt
        /// </summary>
        /// <returns></returns>
        public bool isEncrypted()
        {
            return this.encrypted;
        }

        /// <summary>
        /// Set the return code
        /// </summary>
        /// <param name="returnCode"></param>
        /// <returns></returns>
        public OperationResponse setReturnCode(int returnCode)
        {
            this.returnCode = returnCode;
            return this;
        }

        /// <summary>
        /// Set debug message
        /// </summary>
        /// <param name="debugMessage"></param>
        /// <returns></returns>
        public OperationResponse setDebugMessage(string debugMessage)
        {
            this.debugMessage = debugMessage;
            return this;
        }

        /// <summary>
        /// Set parameter
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public OperationResponse setParameter(int key, object value)
        {
            if (this.parameters == null) this.parameters = new EUNHashtable();

            this.parameters.add(key, value);

            return this;
        }

        /// <summary>
        /// Replace current parameters with param paramters
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public OperationResponse setParameters(EUNHashtable parameters)
        {
            this.parameters = parameters;
            return this;
        }

        /// <summary>
        /// Set the response is encrypt
        /// </summary>
        /// <param name="encrypted"></param>
        /// <returns></returns>
        public OperationResponse setEncrypted(bool encrypted)
        {
            this.encrypted = encrypted;
            return this;
        }

        public string toString()
        {
            var stringBuilder = new System.Text.StringBuilder();

            stringBuilder.Append("Code: " + CodeHelper.getOperationCodeName(this.operationCode) + ", responseId: " + this.responseId + ", returnCode " + CodeHelper.getReturnCodeName(this.returnCode));

            if (this.returnCode == ReturnCode.Ok) stringBuilder.Append(", parameters " + this.parameters);
            else stringBuilder.Append(", debugMessage " + this.debugMessage);

            return stringBuilder.ToString();
        }

        public override string ToString()
        {
            return this.toString();
        }

    }

}
