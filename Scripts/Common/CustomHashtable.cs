namespace EUN.Common
{
#if EUN
    using com.tvd12.ezyfoxserver.client.entity;
    using com.tvd12.ezyfoxserver.client.factory;
#endif

    using System.Collections.Generic;

    public class CustomHashtable
    {
        public class Builder
        {
#if EUN
            private EzyObject ezyObject;
#endif
            public Builder Add(int key, object value)
            {
#if EUN
                if (value is CustomHashtable customHashtable)
                {
                    ezyObject.put(key, customHashtable.toData());

                    return this;
                }

                ezyObject.put(key, value);
#endif
                return this;
            }

            public Builder AddAll(IDictionary<int, object> dict)
            {
#if EUN
                ezyObject.putAll(dict);
#endif
                return this;
            }

            public CustomHashtable Build()
            {
#if EUN
                return new CustomHashtable(ezyObject);
#else
                return null;
#endif
            }

            public Builder()
            {
#if EUN
                ezyObject = EzyEntityFactory.newObject();
#endif
            }
        }

#if EUN
        private EzyObject ezyObject;
#endif

#if EUN
        public CustomHashtable(EzyObject ezyObject)
        {
            this.ezyObject = EzyEntityFactory.newObject();

            if (ezyObject != null)
            {
                var keys = ezyObject.keys();
                foreach (var key in keys)
                {
                    if (key is string keyStr)
                    {
                        int keyInt;
                        if (int.TryParse(keyStr, out keyInt))
                        {
                            this.ezyObject.put(keyInt, ezyObject.get<object>(key));
                        }
                    }
                    else if (key is int keyInt)
                    {
                        this.ezyObject.put(keyInt, ezyObject.get<object>(key));
                    }
                }
            }
        }
#endif

        public CustomHashtable()
        {
#if EUN
            this.ezyObject = EzyEntityFactory.newObject();
#endif
        }

        public void Add(int key, object value)
        {
#if EUN
            if (value is CustomHashtable customHashtable)
            {
                ezyObject.put(key, customHashtable.toData());
                return;
            }

            ezyObject.put(key, value);
#endif
        }

        public ICollection<object> Values()
        {
#if EUN
            return ezyObject.values();
#else 
            return null;
#endif
        }

        public ICollection<object> Keys()
        {
#if EUN
            return ezyObject.keys();
#else
            return null;
#endif
        }

        public bool Remove(int key)
        {
            return false;
        }

        public void Clear()
        {
#if EUN
            ezyObject.clear();
#endif
        }

        public bool ContainsKey(int key)
        {
#if EUN
            return ezyObject.containsKey(key);
#else
            return false;
#endif
        }

        private T Get<T>(int key, T defaultValue = default(T))
        {
#if EUN
            return ezyObject.get<T>(key);
#else
            return defaultValue;
#endif
        }

        public byte GetByte(int key, byte defaultValue = 0)
        {
            return Get(key, defaultValue);
        }

        public byte[] GetByteArr(int key, byte[] defaultValue = null)
        {
            return Get(key, defaultValue);
        }

        public short GetShort(int key, short defaultValue = 0)
        {
            return Get(key, defaultValue);
        }

        public short[] GetShortArr(int key, short[] defaultValue = null)
        {
            return Get(key, defaultValue);
        }

        public int GetInt(int key, int defaultValue = 0)
        {
            return Get(key, defaultValue);
        }

        public int[] GetIntArr(int key, int[] defaultValue = null)
        {
            return Get(key, defaultValue);
        }

        public float GetFloat(int key, float defaultValue = 0)
        {
            return Get(key, defaultValue);
        }

        public float[] GetFloatArr(int key, float[] defaultValue = null)
        {
            return Get(key, defaultValue);
        }

        public long GetLong(int key, long defaultValue = 0)
        {
            return Get(key, defaultValue);
        }

        public long[] GetLongArr(int key, long[] defaultValue = null)
        {
            return Get(key, defaultValue);
        }

        public double GetDouble(int key, double defaultValue = 0)
        {
            return Get(key, defaultValue);
        }

        public double[] GetDoubleArr(int key, double[] defaultValue = null)
        {
            return Get(key, defaultValue);
        }

        public bool GetBool(int key, bool defaultValue = false)
        {
            return Get(key, defaultValue);
        }

        public bool[] GetBoolArr(int key, bool[] defaultValue = null)
        {
            return Get(key, defaultValue);
        }

        public string GetString(int key, string defaultValue = null)
        {
            return Get(key, defaultValue);
        }

        public string[] GetStringArr(int key, string[] defaultValue = null)
        {
            return Get(key, defaultValue);
        }

#if EUN
        public EzyArray GetEzyArray(int key, EzyArray defaultValue = null)
        {
            return Get(key, defaultValue);
        }
#endif

        public object GetObject(int key, object defaultValue = null)
        {
            return Get(key, defaultValue);
        }

#if EUN
        public EzyObject GetEzyObject(int key, EzyObject defaultValue = null)
        {
            return Get(key, defaultValue);
        }
#endif

        public object[] GetObjectArr(int key, object[] defaultValue = null)
        {
            return Get(key, defaultValue);
        }

        public CustomHashtable GetCustomHashtable(int key, CustomHashtable defaultValue = null)
        {
#if EUN
            var ezyObject = Get<EzyObject>(key);
            if (ezyObject != null) return new CustomHashtable(ezyObject);

            return defaultValue;
#else
            return null;
#endif
        }

        public object toData()
        {
#if EUN
            return ezyObject;
#else
            return null;
#endif
        }

        public override string ToString()
        {
            return toData().ToString();
        }
    }
}