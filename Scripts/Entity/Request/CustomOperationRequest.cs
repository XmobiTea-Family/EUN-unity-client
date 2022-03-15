namespace XmobiTea.EUN.Entity.Request
{
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Constant;

    public abstract class CustomOperationRequest
    {
        protected virtual OperationCode Code { get; }

        protected virtual bool Reliable { get; }

        protected int Timeout { get; }
        
        protected CustomHashtable Parameters { get; set; }

        public CustomOperationRequest(int timeout)
        {
            this.Timeout = timeout;
        }

        public OperationRequest Builder()
        {
            var request = new OperationRequest((int)Code, Reliable, Timeout);
            
            if (this.Parameters != null) request.SetParameters(this.Parameters);

            return request;
        }
    }
}