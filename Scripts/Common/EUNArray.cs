namespace XmobiTea.EUN.Common
{
    using System;
    using System.Collections.Generic;

#if EUN_USING_ONLINE
    using com.tvd12.ezyfoxserver.client.factory;
#endif

    public class EUNArray : EUNData
    {
        public class Builder
        {
            private List<object> originArray;

            /// <summary>
            /// Add a value
            /// </summary>
            /// <param name="value">the value</param>
            /// <returns></returns>
            public Builder add(object value)
            {
                this.originArray.Add(value);

                return this;
            }

            /// <summary>
            /// Add all value from a list
            /// </summary>
            /// <param name="list">the list need add all value</param>
            /// <returns></returns>
            public Builder addAll(System.Collections.IList list)
            {
                foreach (var o in list)
                {
                    this.add(o);
                }

                return this;
            }

            /// <summary>
            /// Build an EUNArray from this builder
            /// </summary>
            /// <returns></returns>
            public EUNArray build()
            {
                var awnser = new EUNArray();

                foreach (var o in this.originArray)
                {
                    awnser.add(o);
                }

                return awnser;
            }

            public Builder()
            {
                this.originArray = new List<object>();
            }
        }

        private List<object> originArray;

        public EUNArray()
        {
            this.originArray = new List<object>();
        }

        /// <summary>
        /// Add a value
        /// </summary>
        /// <param name="value">value need add</param>
        public void add(object value)
        {
            this.originArray.Add(this.createUseDataFromOriginData(value));
        }

        /// <summary>
        /// Get all Values this EUNArray
        /// </summary>
        /// <returns>Values of this EUNArray</returns>
        public ICollection<object> values()
        {
            return this.originArray;
        }

        /// <summary>
        /// Clear all key and value in EUNArray
        /// </summary>
        public override void clear()
        {
            this.originArray.Clear();
        }

        /// <summary>
        /// Remove the key
        /// </summary>
        /// <param name="key">the key need get</param>
        /// <returns>true if has key</returns>
        public override bool remove(int k)
        {
            this.originArray.RemoveAt(k);

            return true;
        }

        /// <summary>
        /// Size of EUNArray
        /// </summary>
        /// <returns>size this EUNArray</returns>
        public override int count()
        {
            return this.originArray.Count;
        }

        /// <summary>
        /// Get the object via k index
        /// </summary>
        /// <typeparam name="T">Type of object need get</typeparam>
        /// <param name="k">the key need get</param>
        /// <param name="defaultValue">default value if key does not contains in this EUNArray</param>
        /// <returns>the object or defaultValue or null</returns>
        protected override object get<T>(int k, T defaultValue = default(T))
        {
            if (k < 0 || k > this.originArray.Count - 1) return defaultValue;

            var value = this.originArray[k];

            if (value == null) return defaultValue;

            if (value is T t)
            {
                return t;
            }

            return value;
        }

        /// <summary>
        /// Convert EUNArray to a T[] array as T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T[] toArray<T>()
        {
            return this.originArray.ToArray() as T[];
        }

        /// <summary>
        /// Convert EUNArray to a List<T> as T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IList<T> toList<T>()
        {
            return this.originArray as IList<T>;
        }

        /// <summary>
        /// To Ezy Data
        /// </summary>
        /// <returns>EzyArray from this EUNArray</returns>
        public override object toEzyData()
        {
#if EUN_USING_ONLINE
            var eunArray = EzyEntityFactory.newArray();

            for (var i = 0; i < this.originArray.Count; i++)
            {
                eunArray.add(this.createEUNDataFromUseData(this.originArray[i]));
            }

            return eunArray;
#else
            return null;
#endif
        }

        public override string toString()
        {
            return new System.Text.StringBuilder()
                .Append("[")
                .Append(String.Join(",", this.originArray))
                .Append("]")
                .ToString();
        }

        /// <summary>
        /// To EUNArray string
        /// </summary>
        /// <returns>string like json, but it is EUNArray json</returns>
        public override string ToString()
        {
            return this.toString();
        }

    }

}
