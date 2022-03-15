namespace XmobiTea.EUN.Common
{
#if EUN
    using com.tvd12.ezyfoxserver.client.entity;
#endif

    using System.Collections.Generic;

    public class EUNData : IEUNData
    {
        protected object CreateUseDataFromOriginData(object value)
        {
            if (value == null) return null;

#if EUN
            if (value is EzyArray ezyArray)
            {
                var answer = new EUNArray();

                for (var i = 0; i < ezyArray.size(); i++)
                {
                    answer.Add(ezyArray.get<object>(i));
                }

                return answer;
            }

            if (value is EzyObject ezyObject)
            {
                var answer = new EUNHashtable.Builder().AddAll(ezyObject.toDict<object, object>()).Build();

                return answer;
            }
#endif

            if (value is EUNHashtable eunHashtable)
            {
                return eunHashtable;
            }

            if (value is EUNArray eunArray)
            {
                return eunArray;
            }

            if (value is IList<object> list)
            {
                var answer = new EUNArray();

                for (var i = 0; i < list.Count; i++)
                {
                    answer.Add(list[i]);
                }

                return answer;
            }

            if (value is IDictionary<object, object> dict)
            {
                var answer = new EUNHashtable.Builder().AddAll(dict).Build();

                return answer;
            }

            return value;
        }

        protected object CreateEUNDataFromUseData(object value)
        {
            if (value == null) return null;

            if (value is IEUNData eunData)
            {
                return eunData.ToEzyData();
            }

            return value;
        }

        public virtual void Clear() { }

        public virtual bool Remove(int k) { return false; }

        public virtual int Count() { return 0; }

        protected virtual T Get<T>(int k, T defaultValue = default(T))
        {
            return defaultValue;
        }

        public byte GetByte(int k, byte defaultValue = 0)
        {
            return Get(k, defaultValue);
        }

        public sbyte GetSByte(int k, sbyte defaultValue = 0)
        {
            return Get(k, defaultValue);
        }

        public short GetShort(int k, short defaultValue = 0)
        {
            return Get(k, defaultValue);
        }

        public int GetInt(int k, int defaultValue = 0)
        {
            return Get(k, defaultValue);
        }

        public float GetFloat(int k, float defaultValue = 0)
        {
            return Get(k, defaultValue);
        }

        public long GetLong(int k, long defaultValue = 0)
        {
            return Get(k, defaultValue);
        }

        public double GetDouble(int k, double defaultValue = 0)
        {
            return Get(k, defaultValue);
        }

        public bool GetBool(int k, bool defaultValue = false)
        {
            return Get(k, defaultValue);
        }

        public string GetString(int k, string defaultValue = null)
        {
            return Get(k, defaultValue);
        }

        public object GetObject(int k, object defaultValue = null)
        {
            return Get(k, defaultValue);
        }

        public T[] GetArray<T>(int k, T[] defaultValue = null)
        {
            var value0 = GetEUNArray(k);
            if (value0 != null)
            {
                return value0.ToArray<T>();
            }

            return defaultValue;
        }

        public IList<T> GetList<T>(int k, IList<T> defaultValue = null)
        {
            var value0 = GetEUNArray(k);
            if (value0 != null)
            {
                return value0.ToList<T>();
            }

            return defaultValue;
        }

        public EUNArray GetEUNArray(int k, EUNArray defaultValue = null)
        {
            return Get(k, defaultValue);
        }

        public EUNHashtable GetEUNHashtable(int k, EUNHashtable defaultValue = null)
        {
            return Get(k, defaultValue);
        }

        public virtual object ToEzyData() { return null; }
    }
}
