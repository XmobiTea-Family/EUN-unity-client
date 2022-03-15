namespace XmobiTea.EUN.Common
{
    using XmobiTea.EUN.Constant;

    using System.Text;

    public class OperationEvent
    {
        private int eventCode;
        private CustomHashtable parameters;

        public EventCode GetEventCode()
        {
            return (EventCode)eventCode;
        }

        public CustomHashtable GetParameters()
        {
            return parameters;
        }

        public OperationEvent(int eventCode, CustomHashtable parameters)
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