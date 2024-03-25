namespace XmobiTea.EUN.Entity.Request
{
    using XmobiTea.EUN.Common;

    public abstract class CustomOperationRequest
    {
        /// <summary>
        /// The operation code
        /// </summary>
        protected abstract int code { get; }

        /// <summary>
        /// This reliable or not
        /// It only valid in socket, if not reliable and EUN Client connect by socket, this operation request will send by UDP
        /// </summary>
        protected virtual bool reliable { get; }

        /// <summary>
        /// The timeout in second for this operation request
        /// </summary>
        protected int timeout { get; }
        
        /// <summary>
        /// The parameters for this operation request
        /// </summary>
        protected EUNHashtable parameters { get; set; }

        /// <summary>
        /// CustomOperationRequest
        /// </summary>
        /// <param name="timeout">The time out in second for this request</param>
        public CustomOperationRequest(int timeout)
        {
            this.timeout = timeout;
        }

        //public static explicit operator

        /// <summary>
        /// Build an Operation Request
        /// </summary>
        /// <returns></returns>
        public OperationRequest build()
        {
            var request = new OperationRequest((int)code, reliable, timeout);
            
            if (this.parameters != null) request.setParameters(this.parameters);

            return request;
        }

    }

}
