namespace EUN.Common
{
    using System;
    using System.Collections.Generic;
    using System.Text;

#if EUN
    using com.tvd12.ezyfoxserver.client.factory;
#endif

    public class CustomArray : CustomData
    {
        public class Builder
        {
            private List<object> originArray;

            public Builder Add(object value)
            {
                originArray.Add(value);

                return this;
            }

            public Builder AddAll(IList<object> list)
            {
                foreach (var o in list)
                {
                    Add(o);
                }

                return this;
            }

            public CustomArray Build()
            {
                var awnser = new CustomArray();

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

        public CustomArray()
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

        protected override T Get<T>(int k, T defaultValue = default(T))
        {
            if (k < 0 || k > originArray.Count - 1) return defaultValue;

            var value = originArray[k];

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
            var ezyArray = EzyEntityFactory.newArray();

            for (var i = 0; i < originArray.Count; i++)
            {
                ezyArray.add(CreateEzyDataFromUseData(originArray[i]));
            }

            return ezyArray;
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
