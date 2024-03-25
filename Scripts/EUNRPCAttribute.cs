namespace XmobiTea.EUN
{
    using System;

    /// <summary>
    /// The attribute EUNRPC
    /// This attribute only put on normal method of class extern from EUNBehaviour class
    /// All EUNRPCCommand automation write in EUN-unity-client-custom/Scripts/Constant/EUNRPCCommand.cs
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class EUNRPCAttribute : Attribute { }

}
