namespace XmobiTea.EUN.Entity.Request
{
    using XmobiTea.EUN.Common;

    public abstract class CustomOperationRequest
    {
        /// <summary>
        /// The operation code
        /// </summary>
        protected abstract int Code { get; }

        /// <summary>
        /// This reliable or not
        /// It only valid in socket, if not reliable and EUN Client connect by socket, this operation request will send by UDP
        /// </summary>
        protected virtual bool Reliable { get; }

        /// <summary>
        /// The timeout in second for this operation request
        /// </summary>
        protected int Timeout { get; }
        
        /// <summary>
        /// The parameters for this operation request
        /// </summary>
        protected EUNHashtable Parameters { get; set; }

        /// <summary>
        /// CustomOperationRequest
        /// </summary>
        /// <param name="timeout">The time out in second for this request</param>
        public CustomOperationRequest(int timeout)
        {
            this.Timeout = timeout;
        }

        //public static explicit operator

        /// <summary>
        /// Build an Operation Request
        /// </summary>
        /// <returns></returns>
        public OperationRequest Builder()
        {
            var request = new OperationRequest((int)Code, Reliable, Timeout);
            
            if (this.Parameters != null) request.SetParameters(this.Parameters);

            return request;
        }
    }
}
