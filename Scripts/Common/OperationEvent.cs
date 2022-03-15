namespace XmobiTea.EUN.Common
{
    using XmobiTea.EUN.Constant;

    using System.Text;

    public class OperationEvent
    {
        private int eventCode;
        private EUNHashtable parameters;

        public EventCode GetEventCode()
        {
            return (EventCode)eventCode;
        }

        public EUNHashtable GetParameters()
        {
            return parameters;
        }

        public OperationEvent(int eventCode, EUNHashtable parameters)
        {
            this.eventCode = eventCode;
            this.parameters = parameters;
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append("Code: " + GetEventCode() + " parameters " + parameters);

            return stringBuilder.ToString();
        }
    }
}