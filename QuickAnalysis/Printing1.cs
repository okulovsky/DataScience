using DataScience.QuickAnalysis;
using DataScience.Tables;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataScience
{
    public static class Printing1
    {
        static Dictionary<Type, List<IAccessor>> accessors = new Dictionary<Type, List<IAccessor>>();


        static Dictionary<string,object> FlattenObject(object e)
        {
            var type = e.GetType();
            if (type.IsPrintable())
                return new Dictionary<string, object> { ["Value"] = e };
            if (!accessors.ContainsKey(type))
                accessors[type] = type.GetHierarchicalAccessors().ToList();
            return accessors[type].ToDictionary(z => z.Name, z => z.GetValue(e));
        }

        static IEnumerable<DictionaryEntry> GetEntries(this  IDictionary d)
        {
            foreach (var e in d) yield return (DictionaryEntry)e;
        }

        public static IEnumerable<Dictionary<string, object>> Flatten(object data)
        {
            if (data.GetType().IsPrintable())
            {
                yield return new Dictionary<string, object> { ["Value"] = data };
            }
            else if (data is IEnumerable)
            {
                foreach (var e in ((IEnumerable)data))
                {
                    if (e.GetType().IsPrintable())
                    {
                        yield return FlattenObject(e);
                        continue;
                    }

                    if (e is IDictionary)
                    {
                       
                        yield return ((IDictionary)e)
                            .GetEntries()
                            .ToDictionary(z => z.Key.ToString(), z => z.Value);
                        continue;
                    }
                    if (e is IEnumerable)
                    {
                        yield return ((IEnumerable)e)
                            .Cast<object>()
                            .WithIndices()
                            .ToDictionary(z => z.Index.ToString(), z => z.Item);
                        continue;
                    }

                    yield return FlattenObject(e);
                }
            }
            else
            {
                yield return FlattenObject(data);
            }
        }

        public static IEnumerable<string> ToCsvStrings<T>(this IEnumerable<T> data)
        {
            bool firstTime = true;
            foreach(var e in Flatten(data))
            {
                if (firstTime) yield return string.Join(",", e.Keys);
                firstTime = true;
                yield return string.Join(",", e.Values.Select(z =>
                {
                    if (z == null) return "NA";
                    else if (Types.NullableTypes.Contains(z.GetType()))
                        return z.ToString();
                    else return $"\"{z.ToString()}\"";
                }));
            }
        }

        public static string Print(this object data)
        {

            var table = new Table<int, string, string>();
            var indexer = table.Indexer(z => z.AutoCreate(), z => z.AutoCreate());
            foreach (var row in Flatten(data).WithIndices())
                foreach (var column in row.Item)
                    indexer[row.Index, column.Key] = column.Value?.ToString()??"NULL";

            var lengths = table
                .Columns
                .WithIndices()
                .Select(z =>
                 new
                 {
                     columnIndex = z.Index,
                     columnName = z.Item,
                     max = Math.Max(table.GetColumnData(z.Item).Max(x => x.Value?.Length??0),z.Item.Length)
                 })
                .ToList();

            var formats = lengths
                .Select(coldata => "{" + coldata.columnIndex + ",-" + (coldata.max + 2) + "}")
                .Aggregate((a, b) => a + b);

            var builder = new StringBuilder();
            builder.AppendLine(string.Format(formats, lengths.Select(z=>z.columnName).ToArray()));
            builder.AppendLine(string.Format(formats, lengths.Select(z => new string('-', z.max)).ToArray()));
            foreach (var e in table.Rows)
                builder.AppendLine(string.Format(formats, lengths.Select(z=>table.GetValue(e,z.columnName)).ToArray()));
            var result = builder.ToString();
            Console.WriteLine(result);
            return result;
        }
    }
}
