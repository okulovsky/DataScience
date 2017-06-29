using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataScience
{
    public static class StringTransform
    {
        static string ConvertLetter(ItemWithIndex<string> s)
        {
            if (s.Item == ".") return "_";
            if (s.Item.ToLower() == s.Item) return s.Item;
            if (s.Index != 0) return "_" + s.Item.ToLower();
            return s.Item.ToLower();
        }

        public static string ConvertCamelCase(string s)
        {
            return s
                .Select(x => x.ToString())
                .WithIndices()
                .Select(x => ConvertLetter(x))
                .Join("");
        }
    }


    public class CSVWriter<T>
    {
        List<IAccessor> accessors;
        TextWriter writer;




        public CSVWriter(List<IAccessor> accessors, TextWriter writer)
        {
            this.accessors = accessors;
            this.writer = writer;
            var header =
                accessors.Select(x => StringTransform.ConvertCamelCase(x.Name))
                .Join(",");
            header = header.Replace("__", "_");
            writer.WriteLine(header);
        }

        public void Write(T obj)
        {
            var line = accessors.Select(z =>
            {
                if (obj == null) return null;
                var value = z.GetValue(obj)?.ToString();
                if (value == null)
                    value = "NA";
                else if (!Types.NumericTypes.Contains(z.Type))
                    value = "\"" + value + "\"";
                return value;
            })
            .Join(",");
            writer.WriteLine(line);
        }
    }
}
