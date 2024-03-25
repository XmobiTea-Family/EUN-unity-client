namespace XmobiTea.EUN.Common
{
    using System.Collections.Generic;

    public interface IEUNData
    {
        void clear();

        bool remove(int k);

        int count();

        byte getByte(int k, byte defaultValue = 0);

        sbyte getSByte(int k, sbyte defaultValue = 0);

        short getShort(int k, short defaultValue = 0);

        int getInt(int k, int defaultValue = 0);

        float getFloat(int k, float defaultValue = 0);

        long getLong(int k, long defaultValue = 0);

        double getDouble(int k, double defaultValue = 0);

        bool getBool(int k, bool defaultValue = false);

        string getString(int k, string defaultValue = null);

        object getObject(int k, object defaultValue = null);

        T[] getArray<T>(int k, T[] defaultValue = null);

        IList<T> getList<T>(int k, IList<T> defaultValue = null);

        EUNArray getEUNArray(int k, EUNArray defaultValue = null);

        EUNHashtable getEUNHashtable(int k, EUNHashtable defaultValue = null);

        object toEzyData();

        string toString();

    }

}
