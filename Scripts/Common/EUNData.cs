namespace XmobiTea.EUN.Common
{
#if EUN
    using com.tvd12.ezyfoxserver.client.entity;
#endif

    using System;
    using System.Collections.Generic;

    public class EUNData : IEUNData
    {
        protected static object CreateUseDataFromOriginData(object value)
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
                var answer = new EUNHashtable.Builder()
                    .AddAll(ezyObject.toDict<object, object>() as System.Collections.IDictionary)
                    .Build();

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

            if (value is System.Collections.IList list)
            {
                var answer = new EUNArray();

                for (var i = 0; i < list.Count; i++)
                {
                    answer.Add(list[i]);
                }

                return answer;
            }

            if (value is System.Collections.IDictionary dict)
            {
                var answer = new EUNHashtable.Builder()
                    .AddAll(dict)
                    .Build();

                return answer;
            }

            return value;
        }

        protected static object CreateEUNDataFromUseData(object value)
        {
            if (value == null) return null;

            if (value is IEUNData eunData)
            {
                return eunData.ToEzyData();
            }

            return value;
        }

        /// <summary>
        /// Clear all data in this EUNData
        /// </summary>
        public virtual void Clear() { }

        /// <summary>
        /// Remove a "k" in this EUNData
        /// </summary>
        /// <param name="k"></param>
        /// <returns>true if k exists</returns>
        public virtual bool Remove(int k) { return false; }

        /// <summary>
        /// Size of this EUNData
        /// </summary>
        /// <returns>size of this EUNData</returns>
        public virtual int Count() { return 0; }

        /// <summary>
        /// Get value as T at k pos
        /// </summary>
        /// <typeparam name="T">T type</typeparam>
        /// <param name="k">k pos</param>
        /// <param name="defaultValue">default value if k not valid in EUNData</param>
        /// <returns></returns>
        protected virtual object Get<T>(int k, T defaultValue = default(T))
        {
            return defaultValue;
        }

        /// <summary>
        /// Get byte value at k pos
        /// </summary>
        /// <param name="k"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public byte GetByte(int k, byte defaultValue = 0)
        {
            return Convert.ToByte(Get(k, defaultValue));
        }

        /// <summary>
        /// Get sbyte value at k pos
        /// </summary>
        /// <param name="k"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public sbyte GetSByte(int k, sbyte defaultValue = 0)
        {
            return Convert.ToSByte(Get(k, defaultValue));
        }

        /// <summary>
        /// Get short value at k pos
        /// </summary>
        /// <param name="k"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public short GetShort(int k, short defaultValue = 0)
        {
            return Convert.ToInt16(Get(k, defaultValue));
        }

        /// <summary>
        /// Get int value at k pos
        /// </summary>
        /// <param name="k"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public int GetInt(int k, int defaultValue = 0)
        {
            return Convert.ToInt32(Get(k, defaultValue));
        }

        /// <summary>
        /// Get float value at k pos
        /// </summary>
        /// <param name="k"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public float GetFloat(int k, float defaultValue = 0)
        {
            return Convert.ToSingle(Get(k, defaultValue));
        }

        /// <summary>
        /// Get long value at k pos
        /// </summary>
        /// <param name="k"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public long GetLong(int k, long defaultValue = 0)
        {
            return Convert.ToInt64(Get(k, defaultValue));
        }

        /// <summary>
        /// Get double value at k pos
        /// </summary>
        /// <param name="k"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public double GetDouble(int k, double defaultValue = 0)
        {
            return Convert.ToDouble(Get(k, defaultValue));
        }

        /// <summary>
        /// Get bool value at k pos
        /// </summary>
        /// <param name="k"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public bool GetBool(int k, bool defaultValue = false)
        {
            return Convert.ToBoolean(Get(k, defaultValue));
        }

        /// <summary>
        /// Get string value at k pos
        /// </summary>
        /// <param name="k"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public string GetString(int k, string defaultValue = null)
        {
            return Convert.ToString(Get(k, defaultValue));
        }

        /// <summary>
        /// Get object value at k pos
        /// </summary>
        /// <param name="k"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public object GetObject(int k, object defaultValue = null)
        {
            return Get(k, defaultValue);
        }

        /// <summary>
        /// Get T[] array value at k pos
        /// </summary>
        /// <param name="k"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public T[] GetArray<T>(int k, T[] defaultValue = null)
        {
            var value0 = GetEUNArray(k);
            if (value0 != null)
            {
                return value0.ToArray<T>();
            }

            return defaultValue;
        }

        /// <summary>
        /// Get List<T> value at k pos
        /// </summary>
        /// <param name="k"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public IList<T> GetList<T>(int k, IList<T> defaultValue = null)
        {
            var value0 = GetEUNArray(k);
            if (value0 != null)
            {
                return value0.ToList<T>();
            }

            return defaultValue;
        }

        /// <summary>
        /// Get EUNArray value at k pos
        /// </summary>
        /// <param name="k"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public EUNArray GetEUNArray(int k, EUNArray defaultValue = null)
        {
            return (EUNArray)Get(k, defaultValue);
        }

        /// <summary>
        /// Get EUNHashtable value at k pos
        /// </summary>
        /// <param name="k"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public EUNHashtable GetEUNHashtable(int k, EUNHashtable defaultValue = null)
        {
            return (EUNHashtable)Get(k, defaultValue);
        }

        /// <summary>
        /// Get ezy data for this EUNData
        /// </summary>
        /// <returns></returns>
        public virtual object ToEzyData() { return null; }
    }
}
