namespace XmobiTea.EUN.Entity
{
    using System.Text;
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
        public const int DefaultTimeOut = 15;

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
        private int timeOut;

        /// <summary>
        /// The request is sync or not
        /// If synchronizationRequest is true, the client can not receive callback, and the request id will set -1 after send this request to EUN Server
        /// </summary>
        private bool synchronizationRequest;

        /// <summary>
        /// The timeout end time
        /// </summary>
        private float endTimeOut = -1;

        /// <summary>
        /// The operation code for this operation request
        /// </summary>
        /// <returns></returns>
        public int GetOperationCode()
        {
            return this.operationCode;
        }

        /// <summary>
        /// The request id for this operation request
        /// </summary>
        /// <returns></returns>
        public int GetRequestId()
        {
            return this.requestId;
        }

        /// <summary>
        /// The parameter attactment
        /// </summary>
        /// <returns></returns>
        public EUNHashtable GetParameters()
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
        public bool IsReliable()
        {
            return this.reliable;
        }

        /// <summary>
        /// The timeout in second for this operation request
        /// </summary>
        /// <returns></returns>
        public int GetTimeOut()
        {
            return this.timeOut;
        }

        /// <summary>
        /// The timeout end time
        /// </summary>
        /// <returns></returns>
        public float GetEndTimeOut()
        {
            return this.endTimeOut;
        }

        /// <summary>
        /// The request is sync or not
        /// If synchronizationRequest is true, the client can not receive callback, and the request id will set -1 after send this request to EUN Server
        /// </summary>
        /// <returns></returns>
        public bool IsSynchronizationRequest()
        {
            return this.synchronizationRequest;
        }

        /// <summary>
        /// Set request id for this request
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        public OperationRequest SetRequestId(int requestId)
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
        public OperationRequest SetScriptData(int key, object value)
        {
            if (this.parameters == null) this.parameters = new EUNHashtable();

            this.parameters.Add(key, value);

            return this;
        }

        /// <summary>
        /// Replace all parameters by new parameters for this operation request
        /// </summary>
        /// <param name="newParameters"></param>
        /// <returns></returns>
        public OperationRequest SetParameters(EUNHashtable newParameters)
        {
            this.parameters = newParameters;
            return this;
        }

        /// <summary>
        /// Set reliable for this request
        /// </summary>
        /// <param name="reliable"></param>
        /// <returns></returns>
        public OperationRequest SetReliable(bool reliable)
        {
            this.reliable = reliable;
            return this;
        }

        /// <summary>
        /// The timeout in second for this operation request
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public OperationRequest SetTimeOut(int timeout)
        {
            this.timeOut = timeout;
            return this;
        }

        /// <summary>
        /// Set synchronization for this operation request
        /// </summary>
        /// <param name="synchronizationRequest"></param>
        /// <returns></returns>
        public OperationRequest SetSynchronizationRequest(bool synchronizationRequest)
        {
            this.synchronizationRequest = synchronizationRequest;
            return this;
        }

        /// <summary>
        /// Set end time out for this operation request
        /// </summary>
        /// <param name="endTimeOut"></param>
        /// <returns></returns>
        public OperationRequest SetEndTimeOut(float endTimeOut)
        {
            this.endTimeOut = endTimeOut;
            return this;
        }

        public OperationRequest(int operationCode, bool reliable = true, int timeout = DefaultTimeOut)
        {
            this.operationCode = operationCode;
            this.reliable = reliable;
            this.timeOut = timeout;
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append("Code: " + CodeHelper.GetOperationCodeName(this.operationCode) + " RequestId: " + this.requestId + " parameters " + this.parameters);

            return stringBuilder.ToString();
        }
    }
}
