namespace EUN.Common
{
    using com.tvd12.ezyfoxserver.client.entity;
    using com.tvd12.ezyfoxserver.client.factory;

    using System.Collections.Generic;

    public class CustomHashtable
    {
        private EzyObject ezyObject;

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

        public CustomHashtable()
        {
            this.ezyObject = EzyEntityFactory.newObject();
        }

        public void Add(int key, object value)
        {
            if (value is CustomHashtable customHashtable)
            {
                ezyObject.put(key, customHashtable.ezyObject);
                return;
            }

            ezyObject.put(key, value);
        }

        public ICollection<object> Values()
        {
            return ezyObject.values();
        }

        public ICollection<object> Keys()
        {
            return ezyObject.keys();
        }

        public bool Remove(int key)
        {
            return false;
        }

        public void Clear()
        {
            ezyObject.clear();
        }

        public bool ContainsKey(int key)
        {
            return ezyObject.containsKey(key);
        }

        private T Get<T>(int key, T defaultValue = default(T))
        {
            return ezyObject.get<T>(key);
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

        public EzyArray GetEzyArray(int key, EzyArray defaultValue = null)
        {
            return Get(key, defaultValue);
        }

        public object GetObject(int key, object defaultValue = null)
        {
            return Get(key, defaultValue);
        }

        public EzyObject GetEzyObject(int key, EzyObject defaultValue = null)
        {
            return Get(key, defaultValue);
        }

        public object[] GetObjectArr(int key, object[] defaultValue = null)
        {
            return Get(key, defaultValue);
        }

        public CustomHashtable GetCustomHashtable(int key, CustomHashtable defaultValue = null)
        {
            var ezyObject = Get<EzyObject>(key);
            if (ezyObject != null) return new CustomHashtable(ezyObject);

            return defaultValue;
        }

        public object toData()
        {
            return ezyObject;
        }

        public override string ToString()
        {
            return toData().ToString();
        }
    }
}