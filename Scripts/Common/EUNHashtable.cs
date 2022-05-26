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

            /// <summary>
            /// Add key and value
            /// </summary>
            /// <param name="key">the key</param>
            /// <param name="value">the value</param>
            /// <returns></returns>
            public Builder Add(int key, object value)
            {
                originObject[key] = value;

                return this;
            }

            /// <summary>
            /// Add all key and value from a dictionary
            /// </summary>
            /// <param name="dict">the dict need add</param>
            /// <returns></returns>
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

            /// <summary>
            /// Build an EUNHashtable from this builder
            /// </summary>
            /// <returns></returns>
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

        /// <summary>
        /// Add a key and value
        /// it will replace value if key exists
        /// </summary>
        /// <param name="key">key need add</param>
        /// <param name="value">value need add</param>
        public void Add(int key, object value)
        {
            originObject[key] = CreateUseDataFromOriginData(value);
        }

        /// <summary>
        /// Get all Values this EUNHashtable
        /// </summary>
        /// <returns>Values of this EUNHashtable</returns>
        public ICollection<object> Values()
        {
            return originObject.Values;
        }

        /// <summary>
        /// Get all Keys this EUNHashtable
        /// </summary>
        /// <returns>Keys of this EUNHashtable</returns>
        public ICollection<int> Keys()
        {
            return originObject.Keys;
        }

        /// <summary>
        /// Check if EUNHashtable contains this key
        /// </summary>
        /// <param name="key">the key need check</param>
        /// <returns>true if has key</returns>
        public bool ContainsKey(int key)
        {
            return originObject.ContainsKey(key);
        }

        /// <summary>
        /// Clear all key and value in EUNHashtable
        /// </summary>
        public override void Clear()
        {
            originObject.Clear();
        }

        /// <summary>
        /// Remove the key
        /// </summary>
        /// <param name="key">the key need get</param>
        /// <returns>true if has key</returns>
        public override bool Remove(int key)
        {
            return originObject.Remove(key);
        }

        /// <summary>
        /// Size of EUNHashtable
        /// </summary>
        /// <returns>size this EUNHashtable</returns>
        public override int Count()
        {
            return originObject.Count;
        }

        /// <summary>
        /// Get the object via k key
        /// </summary>
        /// <typeparam name="T">Type of object need get</typeparam>
        /// <param name="k">the key need get</param>
        /// <param name="defaultValue">default value if key does not contains in this EUNHashtable</param>
        /// <returns>the object or defaultValue or null</returns>
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

                return value;
            }

            return defaultValue;
        }

        /// <summary>
        /// To Ezy Data
        /// </summary>
        /// <returns>EzyObject from this EUNHashtable</returns>
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

        /// <summary>
        /// To EUNHashtable string
        /// </summary>
        /// <returns>string like json, but it is EUNHashtable json</returns>
        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append("{");
            var count = 0;
            var commable = originObject.Count - 1;
            var keys = Keys();
            foreach (var key in keys)
            {
                builder
                    .Append(key)
                    .Append(":")
                    .Append(originObject[key]);
                if ((count++) < commable)
                {
                    builder.Append(",");
                }
            }
            builder.Append("}");
            return builder.ToString();
        }
    }
}
