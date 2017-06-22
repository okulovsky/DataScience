using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataScience
{

    

    public class DictionaryAccessor<TKey,TValue> : IAccessor
    {
        internal TKey key;

        public Type Type { get; internal set; }

        public string Name { get; internal set; }

        public object GetValue(object data)
        {
            return (data as IDictionary<TKey, TValue>)[key];
        }

        public DictionaryAccessor(TKey key)
        {
            this.key = key;
            this.Name = key.ToString();
            this.Type = typeof(TValue);
        }

    }
}
