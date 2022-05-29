namespace XmobiTea.EUN.Entity
{
    using System.Text;
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Helper;

    /// <summary>
    /// The operation event
    /// </summary>
    public class OperationEvent
    {
        /// <summary>
        /// The event code for this operation event
        /// </summary>
        private int eventCode;

        /// <summary>
        /// The parameters for this operation event
        /// </summary>
        private EUNHashtable parameters;

        /// <summary>
        /// The event code in EventCode.cs
        /// </summary>
        /// <returns>The event code</returns>
        public int GetEventCode()
        {
            return this.eventCode;
        }

        /// <summary>
        /// The parameters of this event
        /// </summary>
        /// <returns></returns>
        public EUNHashtable GetParameters()
        {
            return this.parameters;
        }

        public OperationEvent(int eventCode, EUNHashtable parameters)
        {
            this.eventCode = eventCode;
            this.parameters = parameters;
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append("Code: " + CodeHelper.GetEventCodeName(this.eventCode) + " parameters " + this.parameters);

            return stringBuilder.ToString();
        }
    }
}
