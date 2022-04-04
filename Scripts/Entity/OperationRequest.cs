namespace XmobiTea.EUN.Entity
{
    using XmobiTea.EUN.Constant;

    using System.Text;
    using XmobiTea.EUN.Common;

    public class OperationRequest
    {
        public const int DefaultTimeOut = 15;

        private int operationCode;
        private int requestId;
        private EUNHashtable parameters;
        private bool reliable;
        private int timeOut;
        private bool synchronizationRequest;

        private float endTimeOut = -1;

        public OperationCode GetOperationCode()
        {
            return (OperationCode)operationCode;
        }

        public int GetRequestId()
        {
            return requestId;
        }

        public EUNHashtable GetParameters()
        {
            return parameters;
        }

        public bool IsReliable()
        {
            return reliable;
        }

        public int GetTimeOut()
        {
            return timeOut;
        }

        public float GetEndTimeOut()
        {
            return endTimeOut;
        }

        public bool IsSynchronizationRequest()
        {
            return synchronizationRequest;
        }

        public OperationRequest SetRequestId(int requestId)
        {
            this.requestId = requestId;
            return this;
        }

        public OperationRequest SetScriptData(int key, object value)
        {
            parameters.Add(key, value);

            return this;
        }

        public OperationRequest SetParameters(EUNHashtable parameters)
        {
            this.parameters = parameters;
            return this;
        }

        public OperationRequest SetReliable(bool reliable)
        {
            this.reliable = reliable;
            return this;
        }

        public OperationRequest SetTimeOut(int timeout)
        {
            this.timeOut = timeout;
            return this;
        }

        public OperationRequest SetSynchronizationRequest(bool synchronizationRequest)
        {
            this.synchronizationRequest = synchronizationRequest;
            return this;
        }

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

            stringBuilder.Append("Code: " + GetOperationCode() + " RequestId: " + requestId + " parameters " + parameters);

            return stringBuilder.ToString();
        }
    }
}