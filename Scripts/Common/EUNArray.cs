namespace XmobiTea.EUN.Common
{
    using System;
    using System.Collections.Generic;
    using System.Text;

#if EUN
    using com.tvd12.ezyfoxserver.client.factory;
#endif

    public class EUNArray : EUNData
    {
        public class Builder
        {
            private List<object> originArray;

            public Builder Add(object value)
            {
                originArray.Add(value);

                return this;
            }

            public Builder AddAll(System.Collections.IList list)
            {
                foreach (var o in list)
                {
                    Add(o);
                }

                return this;
            }

            public EUNArray Build()
            {
                var awnser = new EUNArray();

                foreach (var o in originArray)
                {
                    awnser.Add(o);
                }

                return awnser;
            }

            public Builder()
            {
                originArray = new List<object>();
            }
        }

        private List<object> originArray;

        public EUNArray()
        {
            this.originArray = new List<object>();
        }

        public void Add(object value)
        {
            originArray.Add(CreateUseDataFromOriginData(value));
        }

        public ICollection<object> Values()
        {
            return originArray;
        }

        public override void Clear()
        {
            originArray.Clear();
        }

        public override bool Remove(int index)
        {
            originArray.RemoveAt(index);

            return true;
        }

        public override int Count()
        {
            return originArray.Count;
        }

        protected override object Get<T>(int k, T defaultValue = default(T))
        {
            if (k < 0 || k > originArray.Count - 1) return defaultValue;

            var value = originArray[k];

            if (value == null) return defaultValue;

            if (value is T t)
            {
                return t;
            }

            return defaultValue;
        }

        public T[] ToArray<T>()
        {
            return originArray.ToArray() as T[];
        }

        public IList<T> ToList<T>()
        {
            return originArray as IList<T>;
        }

        public override object ToEzyData()
        {
#if EUN
            var eunArray = EzyEntityFactory.newArray();

            for (var i = 0; i < originArray.Count; i++)
            {
                eunArray.add(CreateEUNDataFromUseData(originArray[i]));
            }

            return eunArray;
#else
            return base.ToEzyData();
#endif
        }

        public override string ToString()
        {
            return new StringBuilder()
                .Append("[")
                .Append(String.Join(",", originArray))
                .Append("]")
                .ToString();
        }
    }
}
