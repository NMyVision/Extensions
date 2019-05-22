using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace NMyVision.Extensions
{
    public static class DictionaryExtensions
    {


        public static void Import(this IDictionary<string, object> current, IDictionary<string, object> source, bool clear = false)
        {
            if (clear) current.Clear();

            source.Each(kvp =>
            {
                if (current.ContainsKey(kvp.Key))
                    current[kvp.Key] = kvp.Value;
                else
                    current.Add(kvp.Key, kvp.Value);
            });
        }

        public static T ToObject<T>(this System.Collections.Specialized.NameValueCollection source) where T : class, new()
        {
            var dict = source.Keys.Cast<string>().ToDictionary(x => x, k => (object)source[k]);

            return ToObject<T>(dict);
        }

        public static T ToObject<T>(this IDictionary<string, object> source, bool forceDefault = true) where T : class, new()
        {
            T someObject = new T();
            Type type = typeof(T);

            foreach (KeyValuePair<string, object> item in source)
            {
                var p = type.GetProperty(item.Key);

                if (p != null)
                {
                    object o = item.Value;
                    if (forceDefault && (!o.IsNullableType() || o.GetType() == typeof(string) && string.IsNullOrEmpty(item.Value.ToString())))
                        o = Activator.CreateInstance(p.PropertyType);

                    p.SetValue(someObject, o.To(p.PropertyType), null);

                }
            }

            return someObject;
        }


        public static IDictionary<string, object> AsDictionary<T>(this T item, BindingFlags bindings = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
            => typeof(T).GetProperties(bindings)
            .Where(p => p.CanRead)
            .ToDictionary(
                pi => pi.Name,
                pi => pi.GetValue(item)
            );
        
        public static IDictionary<string, object> AsDictionary<TInput, TResult>(this TInput item, Expression<Func<TInput, TResult>> e)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            TResult value = e.Compile()(item);

            if (e.Body.NodeType == ExpressionType.MemberAccess)
            {
                MemberExpression member = e.Body as MemberExpression;

                string propertyName = member.Member.Name;

                dic.Add(propertyName, value);
            }
            else if (e.Body.NodeType == ExpressionType.New)
            {
                var pe = (NewExpression)e.Body;

                pe.Type.GetProperties().Zip(
                    pe.Arguments,
                    (p, a) =>
                    {
                        string pName = p.Name;

                        object oValue = null;

                        if (a.NodeType == ExpressionType.MemberAccess)
                        {
                            oValue = typeof(TResult).GetProperty(pName).GetValue(value);
                        }
                        else if (a.NodeType == ExpressionType.Constant)
                        {
                            ConstantExpression constant = a as ConstantExpression;
                            oValue = constant.Value;
                        }

                        return new { ProjectedName = pName, Value = oValue };
                    }).Each(x => dic.Add(x.ProjectedName, x.Value));

            }

            return dic;
        }



        public static ExpandoObject ToExpando(this IDictionary<string, object> dictionary)
        {
            var expando = new ExpandoObject();
            var expandoDic = (IDictionary<string, object>)expando;

            // go through the items in the dictionary and copy over the key value pairs)
            foreach (var kvp in dictionary)
            {
                // if the value can also be turned into an ExpandoObject, then do it!
                if (kvp.Value is IDictionary<string, object>)
                {
                    var expandoValue = ((IDictionary<string, object>)kvp.Value).ToExpando();
                    expandoDic.Add(kvp.Key, expandoValue);
                }
                else if (kvp.Value is System.Collections.ICollection)
                {
                    // iterate through the collection and convert any string-object dictionaries
                    // along the way into expando objects
                    var itemList = new List<object>();
                    foreach (var item in (System.Collections.ICollection)kvp.Value)
                    {
                        if (item is IDictionary<string, object>)
                        {
                            var expandoItem = ((IDictionary<string, object>)item).ToExpando();
                            itemList.Add(expandoItem);
                        }
                        else
                        {
                            itemList.Add(item);
                        }
                    }

                    expandoDic.Add(kvp.Key, itemList);
                }
                else
                {
                    expandoDic.Add(kvp);
                }
            }

            return expando;
        }

        public static ExpandoObject Create(IDictionary<string, object> source)
            => source.ToExpando();

    }
}
