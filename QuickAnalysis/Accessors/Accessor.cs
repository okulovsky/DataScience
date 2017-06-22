using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataScience
{
    public class AccessorFor
    {
        internal AccessorFor parentAccessor;
        internal IAccessor accessor;

        public CombinedAccessor Create()
        {
            var result = new List<IAccessor>();
            var start = this;
            while(start!=null)
            {
                if (start.accessor != null) result.Add(start.accessor);
                start = start.parentAccessor;
            }
            result.Reverse();
            return new CombinedAccessor(result);
        }
    }

    public class AccessorFor<T> : AccessorFor
    {
        public AccessorFor<TValue> At<TValue>(Expression<Func<T, TValue>> address)
        {
            var body = address.Body as MemberExpression;
            IAccessor result;
            if (body.Member is PropertyInfo)
                result = new PropertyAccessor(body.Member as PropertyInfo);
            else
                result = new FieldAccessor(body.Member as FieldInfo);
            return new AccessorFor<TValue> { parentAccessor = this, accessor = result };
        }
    }


    public static class Accessor
    {
        public static AccessorFor<TValue> AtKey<TKey,TValue>(this AccessorFor<Dictionary<TKey,TValue>> accessor, TKey key)
        {
            var result = new DictionaryAccessor<TKey, TValue>(key);
            return new AccessorFor<TValue> { parentAccessor = accessor, accessor = result };
        }
       
       
        public static AccessorFor<T> For<T>()
        {
            return new AccessorFor<T>();
        }

    }
}
