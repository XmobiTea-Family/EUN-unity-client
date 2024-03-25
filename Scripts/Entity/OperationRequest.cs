namespace XmobiTea.EUN.Entity
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Helper;

    /// <summary>
    /// The operation request
    /// </summary>
    public class OperationRequest
    {
        /// <summary>
        /// The default time out
        /// </summary>
        public const int defaultTimeout = 15;

        /// <summary>
        /// The operation code this request
        /// </summary>
        private int operationCode;

        /// <summary>
        /// The request id this operation request
        /// </summary>
        private int requestId;

        /// <summary>
        /// The parameter attactment
        /// </summary>
        private EUNHashtable parameters;
        
        /// <summary>
        /// This request reliable or not
        /// It can not run in WebGL
        /// If reliable is true, the client will send by udp
        /// Default is false
        /// </summary>
        private bool reliable;

        /// <summary>
        /// The timeout in second for this operation request
        /// </summary>
        private int timeout;

        /// <summary>
        /// The request is sync or not
        /// If synchronizationRequest is true, the client can not receive callback, and the request id will set -1 after send this request to EUN Server
        /// </summary>
        private bool synchronizationRequest;

        /// <summary>
        /// The operation code for this operation request
        /// </summary>
        /// <returns></returns>
        public int getOperationCode()
        {
            return this.operationCode;
        }

        /// <summary>
        /// The request id for this operation request
        /// </summary>
        /// <returns></returns>
        public int getRequestId()
        {
            return this.requestId;
        }

        /// <summary>
        /// The parameter attactment
        /// </summary>
        /// <returns></returns>
        public EUNHashtable getParameters()
        {
            return this.parameters;
        }

        /// <summary>
        /// This request reliable or not
        /// It can not run in WebGL
        /// If reliable is true, the client will send by udp
        /// Default is false
        /// </summary>
        /// <returns></returns>
        public bool isReliable()
        {
            return this.reliable;
        }

        /// <summary>
        /// The timeout in second for this operation request
        /// </summary>
        /// <returns></returns>
        public int getTimeout()
        {
            return this.timeout;
        }

        /// <summary>
        /// The request is sync or not
        /// If synchronizationRequest is true, the client can not receive callback, and the request id will set -1 after send this request to EUN Server
        /// </summary>
        /// <returns></returns>
        public bool isSynchronizationRequest()
        {
            return this.synchronizationRequest;
        }

        /// <summary>
        /// Set request id for this request
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        public OperationRequest setRequestId(int requestId)
        {
            this.requestId = requestId;
            return this;
        }

        /// <summary>
        /// Set parameter pair key and object for this operation request
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public OperationRequest setParameter(int key, object value)
        {
            if (this.parameters == null) this.parameters = new EUNHashtable();

            this.parameters.add(key, value);

            return this;
        }

        /// <summary>
        /// Replace all parameters by new parameters for this operation request
        /// </summary>
        /// <param name="newParameters"></param>
        /// <returns></returns>
        public OperationRequest setParameters(EUNHashtable newParameters)
        {
            this.parameters = newParameters;
            return this;
        }

        /// <summary>
        /// Set reliable for this request
        /// </summary>
        /// <param name="reliable"></param>
        /// <returns></returns>
        public OperationRequest setReliable(bool reliable)
        {
            this.reliable = reliable;
            return this;
        }

        /// <summary>
        /// The timeout in second for this operation request
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public OperationRequest setTimeout(int timeout)
        {
            this.timeout = timeout;
            return this;
        }

        /// <summary>
        /// Set synchronization for this operation request
        /// </summary>
        /// <param name="synchronizationRequest"></param>
        /// <returns></returns>
        public OperationRequest setSynchronizationRequest(bool synchronizationRequest)
        {
            this.synchronizationRequest = synchronizationRequest;
            return this;
        }

        public OperationRequest(int operationCode, bool reliable = true, int timeout = defaultTimeout)
        {
            this.operationCode = operationCode;
            this.reliable = reliable;
            this.timeout = timeout;
        }

        public string toString()
        {
            var stringBuilder = new System.Text.StringBuilder();

            stringBuilder.Append("Code: " + CodeHelper.getOperationCodeName(this.operationCode) + " RequestId: " + this.requestId + " parameters " + this.parameters);

            return stringBuilder.ToString();
        }

        public override string ToString()
        {
            return this.toString();
        }

    }

}
