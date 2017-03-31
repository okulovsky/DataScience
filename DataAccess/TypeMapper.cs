using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataScience
{
    public class TypeMapper<T>
    {
        Func<object[], T> maker;

        public T Produce(object[] obj)
        {
            return maker(obj);
        }

        public TypeMapper(params Expression<Func<T,object>>[] fields)
        {
            var parameter = Expression.Parameter(typeof(object[]));
            var binds = new List<MemberBinding>();
            for (int i = 0; i < fields.Length; i++)
            {
                var e = fields[i];

                Expression inp = e.Body;
                if (inp.NodeType == ExpressionType.Convert)
                    inp = ((UnaryExpression)inp).Operand;

                MemberExpression exp = (MemberExpression)inp;
                var member = exp.Member;
                Type type = null;
                if (member is FieldInfo)
                    type = (member as FieldInfo).FieldType;
                else
                    type = (member as PropertyInfo).PropertyType;
                binds.Add(Expression.Bind(member,
                    Expression.Convert(
                        Expression.ArrayAccess(
                            parameter,
                            Expression.Constant(i)),
                        type)));
            }

            var body = Expression.MemberInit(
                Expression.New(typeof(T)),
                binds);

            var lambda  = Expression.Lambda<Func<object[],T>>(body, parameter);

            maker = lambda.Compile();
        }
    }
}
