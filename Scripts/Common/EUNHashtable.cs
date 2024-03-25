namespace XmobiTea.EUN.Common
{
#if EUN_USING_ONLINE
    using com.tvd12.ezyfoxserver.client.factory;
#endif

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
            public Builder add(int key, object value)
            {
                this.originObject[key] = value;

                return this;
            }

            /// <summary>
            /// Add all key and value from a dictionary
            /// </summary>
            /// <param name="dict">the dict need add</param>
            /// <returns></returns>
            public Builder addAll(System.Collections.IDictionary dict)
            {
                var keys = dict.Keys;
                foreach (var key in keys)
                {
                    if (key is string keyStr)
                    {
                        int keyInt;
                        if (int.TryParse(keyStr, out keyInt))
                        {
                            this.add(keyInt, dict[key]);
                        }
                    }
                    else if (key is int keyInt)
                    {
                        this.add(keyInt, dict[key]);
                    }
                }

                return this;
            }

            /// <summary>
            /// Build an EUNHashtable from this builder
            /// </summary>
            /// <returns></returns>
            public EUNHashtable build()
            {
                var awnser = new EUNHashtable();

                var keys = this.originObject.Keys;
                foreach (var key in keys)
                {
                    awnser.add(key, this.originObject[key]);
                }

                return awnser;
            }

            public Builder()
            {
                this.originObject = new Dictionary<int, object>();
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
        public void add(int key, object value)
        {
            this.originObject[key] = this.createUseDataFromOriginData(value);
        }

        /// <summary>
        /// Get all Values this EUNHashtable
        /// </summary>
        /// <returns>Values of this EUNHashtable</returns>
        public ICollection<object> values()
        {
            return this.originObject.Values;
        }

        /// <summary>
        /// Get all Keys this EUNHashtable
        /// </summary>
        /// <returns>Keys of this EUNHashtable</returns>
        public ICollection<int> keys()
        {
            return this.originObject.Keys;
        }

        /// <summary>
        /// Check if EUNHashtable contains this key
        /// </summary>
        /// <param name="key">the key need check</param>
        /// <returns>true if has key</returns>
        public bool containsKey(int key)
        {
            return this.originObject.ContainsKey(key);
        }

        /// <summary>
        /// Clear all key and value in EUNHashtable
        /// </summary>
        public override void clear()
        {
            this.originObject.Clear();
        }

        /// <summary>
        /// Remove the key
        /// </summary>
        /// <param name="key">the key need get</param>
        /// <returns>true if has key</returns>
        public override bool remove(int key)
        {
            return this.originObject.Remove(key);
        }

        /// <summary>
        /// Size of EUNHashtable
        /// </summary>
        /// <returns>size this EUNHashtable</returns>
        public override int count()
        {
            return this.originObject.Count;
        }

        /// <summary>
        /// Get the object via k key
        /// </summary>
        /// <typeparam name="T">Type of object need get</typeparam>
        /// <param name="k">the key need get</param>
        /// <param name="defaultValue">default value if key does not contains in this EUNHashtable</param>
        /// <returns>the object or defaultValue or null</returns>
        protected override object get<T>(int k, T defaultValue = default(T))
        {
            if (this.originObject.ContainsKey(k))
            {
                var value = this.originObject[k];

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
        public override object toEzyData()
        {
#if EUN_USING_ONLINE
            var ezyObject = EzyEntityFactory.newObject();

            var keys = originObject.Keys;
            foreach (var key in keys)
            {
                ezyObject.put(key, this.createEUNDataFromUseData(this.originObject[key]));
            }

            return ezyObject;
#else
            return null;
#endif
        }

        public override string toString()
        {
            var builder = new StringBuilder();
            builder.Append("{");
            var count = 0;
            var commable = this.originObject.Count - 1;
            var keys = this.keys();
            foreach (var key in keys)
            {
                builder
                    .Append(key)
                    .Append(":")
                    .Append(this.originObject[key]);
                if ((count++) < commable)
                {
                    builder.Append(",");
                }
            }
            builder.Append("}");
            return builder.ToString();
        }

        /// <summary>
        /// To EUNHashtable string
        /// </summary>
        /// <returns>string like json, but it is EUNHashtable json</returns>
        public override string ToString()
        {
            return this.toString();
        }

    }

}
