namespace XmobiTea.EUN.Common
{
    using System.Collections.Generic;

    public interface IEUNData
    {
        void Clear();

        bool Remove(int k);

        int Count();

        byte GetByte(int k, byte defaultValue = 0);

        sbyte GetSByte(int k, sbyte defaultValue = 0);

        short GetShort(int k, short defaultValue = 0);

        int GetInt(int k, int defaultValue = 0);

        float GetFloat(int k, float defaultValue = 0);

        long GetLong(int k, long defaultValue = 0);

        double GetDouble(int k, double defaultValue = 0);

        bool GetBool(int k, bool defaultValue = false);

        string GetString(int k, string defaultValue = null);

        object GetObject(int k, object defaultValue = null);

        T[] GetArray<T>(int k, T[] defaultValue = null);

        IList<T> GetList<T>(int k, IList<T> defaultValue = null);

        EUNArray GetEUNArray(int k, EUNArray defaultValue = null);

        EUNHashtable GetEUNHashtable(int k, EUNHashtable defaultValue = null);

        object ToEzyData();
    }
}
