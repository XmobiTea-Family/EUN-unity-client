namespace XmobiTea.EUN.Common
{
#if EUN
    using com.tvd12.ezyfoxserver.client.factory;
#endif

    using System;
    using System.Collections.Generic;
    using System.Text;

    public class EUNHashtable : EUNData
    {
        public class Builder
        {
            private IDictionary<int, object> originObject;

            public Builder Add(int key, object value)
            {
                originObject[key] = value;

                return this;
            }

            public Builder AddAll(System.Collections.IDictionary dict)
            {
                var keys = dict.Keys;
                foreach (var key in keys)
                {
                    if (key is string keyStr)
                    {
                        int keyInt;
                        if (int.TryParse(keyStr, out keyInt))
                        {
                            Add(keyInt, dict[key]);
                        }
                    }
                    else if (key is int keyInt)
                    {
                        Add(keyInt, dict[key]);
                    }
                }

                return this;
            }

            public EUNHashtable Build()
            {
                var awnser = new EUNHashtable();

                var keys = originObject.Keys;
                foreach (var key in keys)
                {
                    awnser.Add(key, originObject[key]);
                }

                return awnser;
            }

            public Builder()
            {
                originObject = new Dictionary<int, object>();
            }
        }

        private IDictionary<int, object> originObject;

        public EUNHashtable()
        {
            this.originObject = new Dictionary<int, object>();
        }

        public void Add(int key, object value)
        {
            originObject[key] = CreateUseDataFromOriginData(value);
        }

        public ICollection<object> Values()
        {
            return originObject.Values;
        }

        public ICollection<int> Keys()
        {
            return originObject.Keys;
        }

        public bool ContainsKey(int key)
        {
            return originObject.ContainsKey(key);
        }

        public override void Clear()
        {
            originObject.Clear();
        }

        public override bool Remove(int key)
        {
            return originObject.Remove(key);
        }

        public override int Count()
        {
            return originObject.Count;
        }

        protected override object Get<T>(int k, T defaultValue = default(T))
        {
            if (originObject.ContainsKey(k))
            {
                var value = originObject[k];

                if (value == null) return defaultValue;

                if (value is T t)
                {
                    return t;
                }

                return defaultValue;
            }

            return defaultValue;
        }

        public override object ToEzyData()
        {
#if EUN
            var ezyObject = EzyEntityFactory.newObject();

            var keys = originObject.Keys;
            foreach (var key in keys)
            {
                ezyObject.put(key, CreateEUNDataFromUseData(originObject[key]));
            }

            return ezyObject;
#else
            return base.ToEzyData();
#endif
        }

        public override string ToString()
        {
            return new StringBuilder()
                .Append("[")
                .Append(String.Join(",", originObject))
                .Append("]")
                .ToString();
        }
    }
}