namespace XmobiTea.EUN.Entity
{
    using System.Text;
    using XmobiTea.EUN.Common;
    using XmobiTea.EUN.Helper;

    public class OperationEvent
    {
        private int eventCode;
        private EUNHashtable parameters;

        public int GetEventCode()
        {
            return eventCode;
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

            stringBuilder.Append("Code: " + CodeHelper.GetEventCodeName(eventCode) + " parameters " + parameters);

            return stringBuilder.ToString();
        }
    }
}
