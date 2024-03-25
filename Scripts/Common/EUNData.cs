namespace XmobiTea.EUN.Common
{
#if EUN_USING_ONLINE
    using com.tvd12.ezyfoxserver.client.entity;
#endif

    using System;
    using System.Collections.Generic;

    public abstract class EUNData : IEUNData
    {
        protected object createUseDataFromOriginData(object value)
        {
            if (value == null) return null;

#if EUN_USING_ONLINE
            if (value is EzyArray ezyArray)
            {
                var answer = new EUNArray();

                for (var i = 0; i < ezyArray.size(); i++)
                {
                    answer.add(ezyArray.get<object>(i));
                }

                return answer;
            }

            if (value is EzyObject ezyObject)
            {
                var answer = new EUNHashtable.Builder()
                    .addAll(ezyObject.toDict<object, object>() as System.Collections.IDictionary)
                    .build();

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
                    answer.add(list[i]);
                }

                return answer;
            }

            if (value is System.Collections.IDictionary dict)
            {
                var answer = new EUNHashtable.Builder()
                    .addAll(dict)
                    .build();

                return answer;
            }

            return value;
        }

        protected object createEUNDataFromUseData(object value)
        {
            if (value == null) return null;

            if (value is IEUNData eunData)
            {
                return eunData.toEzyData();
            }

            return value;
        }

        /// <summary>
        /// Clear all data in this EUNData
        /// </summary>
        public virtual void clear() { }

        /// <summary>
        /// Remove a "k" in this EUNData
        /// </summary>
        /// <param name="k"></param>
        /// <returns>true if k exists</returns>
        public virtual bool remove(int k) { return false; }

        /// <summary>
        /// Size of this EUNData
        /// </summary>
        /// <returns>size of this EUNData</returns>
        public virtual int count() { return 0; }

        /// <summary>
        /// Get value as T at k pos
        /// </summary>
        /// <typeparam name="T">T type</typeparam>
        /// <param name="k">k pos</param>
        /// <param name="defaultValue">default value if k not valid in EUNData</param>
        /// <returns></returns>
        protected virtual object get<T>(int k, T defaultValue = default(T))
        {
            return defaultValue;
        }

        /// <summary>
        /// Get byte value at k pos
        /// </summary>
        /// <param name="k"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public byte getByte(int k, byte defaultValue = 0)
        {
            return Convert.ToByte(this.get(k, defaultValue));
        }

        /// <summary>
        /// Get sbyte value at k pos
        /// </summary>
        /// <param name="k"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public sbyte getSByte(int k, sbyte defaultValue = 0)
        {
            return Convert.ToSByte(this.get(k, defaultValue));
        }

        /// <summary>
        /// Get short value at k pos
        /// </summary>
        /// <param name="k"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public short getShort(int k, short defaultValue = 0)
        {
            return Convert.ToInt16(this.get(k, defaultValue));
        }

        /// <summary>
        /// Get int value at k pos
        /// </summary>
        /// <param name="k"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public int getInt(int k, int defaultValue = 0)
        {
            return Convert.ToInt32(this.get(k, defaultValue));
        }

        /// <summary>
        /// Get float value at k pos
        /// </summary>
        /// <param name="k"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public float getFloat(int k, float defaultValue = 0)
        {
            return Convert.ToSingle(this.get(k, defaultValue));
        }

        /// <summary>
        /// Get long value at k pos
        /// </summary>
        /// <param name="k"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public long getLong(int k, long defaultValue = 0)
        {
            return Convert.ToInt64(this.get(k, defaultValue));
        }

        /// <summary>
        /// Get double value at k pos
        /// </summary>
        /// <param name="k"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public double getDouble(int k, double defaultValue = 0)
        {
            return Convert.ToDouble(this.get(k, defaultValue));
        }

        /// <summary>
        /// Get bool value at k pos
        /// </summary>
        /// <param name="k"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public bool getBool(int k, bool defaultValue = false)
        {
            return Convert.ToBoolean(this.get(k, defaultValue));
        }

        /// <summary>
        /// Get string value at k pos
        /// </summary>
        /// <param name="k"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public string getString(int k, string defaultValue = null)
        {
            return Convert.ToString(this.get(k, defaultValue));
        }

        /// <summary>
        /// Get object value at k pos
        /// </summary>
        /// <param name="k"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public object getObject(int k, object defaultValue = null)
        {
            return this.get(k, defaultValue);
        }

        /// <summary>
        /// Get T[] array value at k pos
        /// </summary>
        /// <param name="k"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public T[] getArray<T>(int k, T[] defaultValue = null)
        {
            var value0 = this.getEUNArray(k);
            if (value0 != null)
            {
                return value0.toArray<T>();
            }

            return defaultValue;
        }

        /// <summary>
        /// Get List<T> value at k pos
        /// </summary>
        /// <param name="k"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public IList<T> getList<T>(int k, IList<T> defaultValue = null)
        {
            var value0 = this.getEUNArray(k);
            if (value0 != null)
            {
                return value0.toList<T>();
            }

            return defaultValue;
        }

        /// <summary>
        /// Get EUNArray value at k pos
        /// </summary>
        /// <param name="k"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public EUNArray getEUNArray(int k, EUNArray defaultValue = null)
        {
            return (EUNArray)this.get(k, defaultValue);
        }

        /// <summary>
        /// Get EUNHashtable value at k pos
        /// </summary>
        /// <param name="k"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public EUNHashtable getEUNHashtable(int k, EUNHashtable defaultValue = null)
        {
            return (EUNHashtable)this.get(k, defaultValue);
        }

        /// <summary>
        /// Get ezy data for this EUNData
        /// </summary>
        /// <returns></returns>
        public abstract object toEzyData();

        public abstract string toString();
    }

}
